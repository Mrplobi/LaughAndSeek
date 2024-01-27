using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    Clown _clown;

    [SerializeField]
    List<LevelScriptable> levelScripts;

    int currentLevel = 0;

    private void Start()
    {
        _clown.onHit += ClownFound;
        StartRound();
    }

    void StartRound()
    {
        if (currentLevel >= levelScripts.Count)
            return;

        var spawns = levelScripts[currentLevel].levelSpawnNames;
        string spawnChosen = spawns[Random.Range(0, spawns.Count)];
        SpawnClown(spawnChosen);
    }

    public void SpawnClown(string spawnName)
    {
        var spawn = GameObject.Find(spawnName);
        _clown.Spawn(spawn.transform);
    }

    public void ClownFound()
    {
        currentLevel++;
        StartRound();
    }
}
