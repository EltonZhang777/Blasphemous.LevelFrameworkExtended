using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Tools.Level.Layout;
using UnityEngine;

namespace MonsterEditor.Components;

/// <summary>
/// Turns a cloned spawn point into an ordinary "spawn on load" point, so a level edit can
/// use the monster catalogue without the in-game editor.
/// </summary>
internal sealed class MonsterPlacementModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = string.IsNullOrEmpty(data.id) ? "MonsterSpawn" : data.id;

        EnemySpawnPoint point = obj.GetComponent<EnemySpawnPoint>();
        if (point == null)
        {
            obj.SetActive(false);
            return;
        }

        point.SpawnEnabledEnemy = true;
        point.SpawnOnArena = false;
        point.EnableBehaviourOnLoad = true;
        point.EnablePersistence = false;
        point.EnemySpawnDisabled = false;

        Transform spawnPoint = obj.transform.Find("Spawn Point");
        if (spawnPoint != null)
        {
            spawnPoint.localPosition = Vector3.zero;
        }

        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
    }
}