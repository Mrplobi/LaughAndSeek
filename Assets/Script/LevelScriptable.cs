using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "Scriptables/Level")]
public class LevelScriptable : ScriptableObject
{
    public List<SpawnInfo> levelSpawns = new List<SpawnInfo>();

    public SpawnInfo GetRandomSpawn()
    {
        return levelSpawns[Random.Range(0, levelSpawns.Count)];
    }
}

[System.Serializable]
public class SpawnInfo
{
    public string spawnName;
    public Vector3 spawnOffset;
    public Vector3 spawnScale = Vector3.one;
}
