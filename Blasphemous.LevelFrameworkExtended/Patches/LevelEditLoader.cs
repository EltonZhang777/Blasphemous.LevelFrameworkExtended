using Blasphemous.Framework.Levels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Blasphemous.LevelFrameworkExtended.Patches;

/// <summary>
/// Loads level edit files from a folder, supporting both .json and .jsonc files.
/// This is the isolated test seam: a pure function with no Harmony or Unity dependencies.
/// </summary>
public static class LevelEditLoader
{
    internal const string LevelEditTypeName = "Blasphemous.Framework.Levels.LevelEdit";

    /// <summary>
    /// Loads every .json and .jsonc file in the folder into a Dictionary of scene key -> LevelEdit.
    /// .json files take priority over same-named .jsonc files. Files that fail to parse are
    /// skipped and reported through the provided log callbacks.
    /// </summary>
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

    private static void TryAddEdit(IDictionary edits, string path, Type levelEditType, JsonLoadSettings settings, Action<string> logError)
    {
        try
        {
            JObject json = JObject.Parse(File.ReadAllText(path), settings);
            edits.Add(Path.GetFileNameWithoutExtension(path), json.ToObject(levelEditType));
        }
        catch (Exception e)
        {
            logError($"Failed to load level edit file '{path}': {e}");
        }
    }
}
