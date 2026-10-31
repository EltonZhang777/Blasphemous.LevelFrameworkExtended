namespace MonsterEditor.Components;

/// <summary>What a catalogue entry spawns.</summary>
internal enum MonsterKind
{
    /// <summary>An ordinary enemy, cloned from an <c>EnemySpawnPoint</c>.</summary>
    Enemy,

    /// <summary>A bare <c>ReekSpawnPoint</c> marker, registered with a ReekLeader.</summary>
    ReekPoint,

    /// <summary>A bare <c>MeltedLadyTeleportPoint</c> marker.</summary>
    MeltedLadyPoint,
}

/// <summary>
/// One editable monster: its catalogue id, display name, and the spawn-point template it
/// is cloned from.
/// </summary>
internal sealed class MonsterEntry
{
    internal MonsterEntry(string id, string name, string scene, string path, float lift)
        : this(id, name, scene, path, lift, MonsterKind.Enemy)
    {
    }

    internal MonsterEntry(string id, string name, string scene, string path, float lift, MonsterKind kind)
    {
        Id = id;
        Name = name;
        Scene = scene;
        Path = path;
        Lift = lift;
        Kind = kind;
    }

    /// <summary>Gets the catalogue id, for example <c>EN04</c>.</summary>
    internal string Id { get; }

    /// <summary>Gets the display name shown in the editor.</summary>
    internal string Name { get; }

    /// <summary>Gets the source logic scene that holds a ready-made spawn point.</summary>
    internal string Scene { get; }

    /// <summary>Gets the hierarchy path of that spawn point inside the scene.</summary>
    internal string Path { get; }

    /// <summary>Gets the vertical offset so ground monsters start above the floor.</summary>
    internal float Lift { get; }

    /// <summary>Gets what this entry spawns.</summary>
    internal MonsterKind Kind { get; }
}