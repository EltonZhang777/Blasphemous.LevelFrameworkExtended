using Blasphemous.Framework.Levels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Blasphemous.LevelFrameworkExtended.Patches;

/// <summary>
/// Loads level edit files from a folder, supporting both .json and .jsonc files.
/// This is the isolated test seam: a pure function with no Harmony or Unity dependencies.
/// </summary>
public static class LevelEditLoader
{
    /// <summary>
    /// The three level edit arrays defined by the upstream <c>LevelEdit</c> type.
    /// JSON documents only carry these arrays at the top level.
    /// </summary>
    internal enum LevelEditType
    {
        /// <summary>Objects to add to the scene.</summary>
        Additions,
        /// <summary>Objects to modify in the scene.</summary>
        Modifications,
        /// <summary>Objects to delete from the scene.</summary>
        Deletions
    }

    /// <summary>
    /// Fully-qualified name of the framework's internal <c>LevelEdit</c> type.
    /// </summary>
    internal const string LevelEditTypeName = "Blasphemous.Framework.Levels.LevelEdit";

    /// <summary>
    /// Loads every .json and .jsonc file in the folder into a Dictionary of scene key -> LevelEdit.
    /// .json files take priority over same-named .jsonc files. Files that fail to parse are
    /// skipped and reported through the provided log callbacks.
    /// </summary>
    /// <param name="folder">Path to the mod's levels folder containing the edit files.</param>
    /// <param name="logError">Callback invoked with an error message when a file fails to load.</param>
    /// <param name="logWarning">Callback invoked with a warning message for non-fatal conflicts.</param>
    /// <returns>A dictionary mapping scene keys to parsed LevelEdit documents.</returns>
    public static IDictionary LoadLevelEdits(string folder, Action<string> logError, Action<string> logWarning)
    {
        Type levelEditType = typeof(LevelFramework).Assembly.GetType(LevelEditTypeName)
            ?? throw new InvalidOperationException($"Could not find type '{LevelEditTypeName}'");

        IDictionary edits = (IDictionary)Activator.CreateInstance(
            typeof(Dictionary<,>).MakeGenericType(typeof(string), levelEditType));
        JsonLoadSettings settings = new() { CommentHandling = CommentHandling.Ignore };

        List<string> jsonFiles = [];
        List<string> jsoncFiles = [];
        foreach (string path in Directory.GetFiles(folder))
        {
            string extension = Path.GetExtension(path);
            if (extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                jsonFiles.Add(path);
            else if (extension.Equals(".jsonc", StringComparison.OrdinalIgnoreCase))
                jsoncFiles.Add(path);
        }

        foreach (string path in jsonFiles)
            TryAddEdit(edits, path, levelEditType, settings, logError);

        foreach (string path in jsoncFiles)
        {
            string key = Path.GetFileNameWithoutExtension(path);
            if (edits.Contains(key))
            {
                logWarning($"Ignoring '{Path.GetFileName(path)}' because a .json file with the same scene already exists");
                continue;
            }
            TryAddEdit(edits, path, levelEditType, settings, logError);
        }

        return edits;
    }

    /// <summary>
    /// Parses a single level edit file, expands any <c>$prev</c> entries, and adds the
    /// resulting LevelEdit to the edits dictionary under its scene key.
    /// Failures are isolated: the file is skipped and reported, other files are unaffected.
    /// </summary>
    /// <param name="edits">The dictionary the parsed edit is added to.</param>
    /// <param name="path">Full path of the edit file to load.</param>
    /// <param name="levelEditType">The framework's <c>LevelEdit</c> type, resolved via reflection.</param>
    /// <param name="settings">JsonLoadSettings used to parse the file (comments ignored).</param>
    /// <param name="logError">Callback invoked with an error message when this file fails.</param>
    /// <returns>This method does not return a value.</returns>
    private static void TryAddEdit(IDictionary edits, string path, Type levelEditType, JsonLoadSettings settings, Action<string> logError)
    {
        try
        {
            JObject json = JObject.Parse(File.ReadAllText(path), settings);
            json = ExpandLevelEdit(json, logError, Path.GetFileName(path));
            edits.Add(Path.GetFileNameWithoutExtension(path), json.ToObject(levelEditType));
        }
        catch (Exception e)
        {
            logError($"Failed to load level edit file '{path}': {e}");
        }
    }

    /// <summary>
    /// The special <c>type</c> value marking an entry that inherits from the previous entry
    /// in the same array. The <c>$</c> prefix reserves this name for syntax, never a real type.
    /// </summary>
    internal const string PrevType = "$prev";

    /// <summary>
    /// Matches a Windows-style copy suffix such as " (2)" at the end of an id.
    /// </summary>
    private static readonly Regex _copySuffix = new(@" \(\d+\)$");

    /// <summary>
    /// Expands every <c>$prev</c> entry in the level edit into a full copy of the previous
    /// entry in the same array, plus any explicitly overridden fields. Runs entirely in memory;
    /// the input document is never modified.
    /// </summary>
    /// <param name="json">The parsed level edit document to expand.</param>
    /// <param name="logError">Callback invoked with an error message when a <c>$prev</c> entry
    /// has no previous entry to inherit from.</param>
    /// <param name="sourceFileName">Name of the source file, used in error messages.</param>
    /// <returns>A new expanded document; the input is left untouched.</returns>
    public static JObject ExpandLevelEdit(JObject json, Action<string> logError, string sourceFileName)
    {
        JObject result = (JObject)json.DeepClone();
        HashSet<string> occupancy = CollectIds(result);

        ExpandArray(result, LevelEditType.Additions, occupancy, renameIds: true, logError, sourceFileName);
        ExpandArray(result, LevelEditType.Modifications, occupancy, renameIds: false, logError, sourceFileName);
        ExpandArray(result, LevelEditType.Deletions, occupancy, renameIds: false, logError, sourceFileName);
        return result;
    }

    /// <summary>
    /// Expands <c>$prev</c> entries within a single array (additions, modifications, or deletions),
    /// keeping an independent "previous entry" per array. A <c>$prev</c> with no previous entry is
    /// removed and reported, without interrupting the rest of the array.
    /// </summary>
    /// <param name="result">The expanded document whose array is being processed.</param>
    /// <param name="levelEditType">Which of the three level edit arrays to process.</param>
    /// <param name="occupancy">Set of ids already in use, used for id renaming.</param>
    /// <param name="renameIds">Whether inherited ids in this array get renamed; true for additions only.</param>
    /// <param name="logError">Callback invoked with an error message for orphan <c>$prev</c> entries.</param>
    /// <param name="sourceFileName">Name of the source file, used in error messages.</param>
    /// <returns>This method does not return a value.</returns>
    private static void ExpandArray(
        JObject result, 
        LevelEditType levelEditType, 
        HashSet<string> occupancy, 
        bool renameIds, 
        Action<string> logError, 
        string sourceFileName)
    {
        string levelEditName = LevelEditTypeToString(levelEditType);
        if (result[levelEditName] is not JArray levelEdits)
            return;

        JObject previous = null;
        for (int i = 0; i < levelEdits.Count; i++)
        {
            if (levelEdits[i] is not JObject levelEditEntry)
                continue;

            if (!IsPrev(levelEditEntry))
            {
                previous = levelEditEntry;
                continue;
            }

            if (previous == null)
            {
                logError($"Failed to expand '$prev' entry in '{sourceFileName}' {levelEditName}[{i}]: no previous entry");
                levelEdits.RemoveAt(i);
                i--;
                continue;
            }

            JObject expanded = MergeIntoCopy(previous, levelEditEntry, occupancy, renameIds);
            levelEdits[i] = expanded;
            previous = expanded;
        }
    }

    /// <summary>
    /// Determines whether an entry is a <c>$prev</c> reference, by checking its <c>type</c> value.
    /// </summary>
    /// <param name="entry">The JSON entry to inspect.</param>
    /// <returns><c>true</c> if the entry's type equals <c>$prev</c>; otherwise <c>false</c>.</returns>
    private static bool IsPrev(JObject entry) =>
        entry["type"] is JValue type && string.Equals((string)type, PrevType, StringComparison.Ordinal);

    /// <summary>
    /// Builds the expanded entry: a deep copy of the previous entry overridden by every field
    /// explicitly present in the <c>$prev</c> entry. Vector fields are merged per axis;
    /// properties are replaced wholesale; inherited ids are renamed when requested.
    /// </summary>
    /// <param name="previous">The previous entry (already expanded) to inherit from.</param>
    /// <param name="entry">The raw <c>$prev</c> entry providing explicit overrides.</param>
    /// <param name="occupancy">Set of ids already in use; new ids are added to it.</param>
    /// <param name="renameIds">Whether an inherited id should be renamed to a unique value.</param>
    /// <returns>The merged, fully expanded entry.</returns>
    private static JObject MergeIntoCopy(JObject previous, JObject entry, HashSet<string> occupancy, bool renameIds)
    {
        JObject merged = (JObject)previous.DeepClone();

        foreach (string field in new[] { "scene", "path", "condition", "properties" })
        {
            if (entry.ContainsKey(field))
                merged[field] = entry[field].DeepClone();
        }

        foreach (string field in new[] { "position", "rotation", "scale" })
        {
            if (entry.ContainsKey(field))
                merged[field] = entry[field] is JObject entryVector
                    ? merged[field] is JObject prevVector
                        ? MergeVector(prevVector, entryVector)
                        : entryVector.DeepClone()
                    : entry[field].DeepClone();
        }

        if (entry.ContainsKey("id"))
        {
            merged["id"] = entry["id"].DeepClone();
        }
        else if (renameIds && merged["id"] is JValue idValue && !string.IsNullOrEmpty((string)idValue))
        {
            string unique = NextUniqueId((string)idValue, occupancy);
            merged["id"] = unique;
            occupancy.Add(unique);
        }

        return merged;
    }

    /// <summary>
    /// Merges an override vector into an inherited one: axes present in <c>overrides</c> replace
    /// the inherited values, absent axes are kept.
    /// </summary>
    /// <param name="target">The inherited vector (x/y/z object) to merge into.</param>
    /// <param name="overrides">The explicit vector providing axis-level overrides.</param>
    /// <returns>A new vector object with overrides applied.</returns>
    private static JObject MergeVector(JObject target, JObject overrides)
    {
        JObject result = target == null 
            ? [] 
            : (JObject)target.DeepClone();
        foreach (var axis in overrides)
            result[axis.Key] = axis.Value.DeepClone();
        return result;
    }

    /// <summary>
    /// Maps a level edit array type to its JSON property name in the document.
    /// </summary>
    /// <param name="type">The level edit array type.</param>
    /// <returns>The JSON property name: additions, modifications, or deletions.</returns>
    private static string LevelEditTypeToString(LevelEditType type) => type switch
    {
        LevelEditType.Additions => "additions",
        LevelEditType.Modifications => "modifications",
        LevelEditType.Deletions => "deletions",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    /// <summary>
    /// Collects every non-empty <c>id</c> across all three arrays of the document, so generated
    /// copy ids can avoid colliding with any real id (including ids in later entries).
    /// </summary>
    /// <param name="json">The expanded document to scan.</param>
    /// <returns>The set of all real ids found in the document.</returns>
    private static HashSet<string> CollectIds(JObject json)
    {
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (LevelEditType type in Enum.GetValues(typeof(LevelEditType)))
        {
            if (json[LevelEditTypeToString(type)] is not JArray array)
                continue;
            foreach (JToken token in array)
            {
                if (token is JObject entry && entry["id"] is JValue id && !string.IsNullOrEmpty((string)id))
                    ids.Add((string)id);
            }
        }
        return ids;
    }

    /// <summary>
    /// Generates a unique copy id in Windows style: strips any existing " (n)" suffix to find
    /// the base id, then appends the lowest " (n)" not already in the occupancy set.
    /// </summary>
    /// <param name="id">The inherited id to make unique.</param>
    /// <param name="occupancy">Set of ids already in use; the returned id avoids them all.</param>
    /// <returns>A unique id of the form "base (n)".</returns>
    private static string NextUniqueId(string id, HashSet<string> occupancy)
    {
        string baseId = _copySuffix.Replace(id, "");
        string candidate = $"{baseId} (2)";
        for (int n = 3; occupancy.Contains(candidate); n++)
            candidate = $"{baseId} ({n})";
        return candidate;
    }
}
