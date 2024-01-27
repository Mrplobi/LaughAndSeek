using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Clown _clown;

    [SerializeField]
    List<LevelScriptable> levelScripts;

    int currentLevel = 0;

    private void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        if (currentLevel >= levelScripts.Count)
            return;

        var selectedClown = levelScripts[currentLevel].allowedClowns[Random.Range(0, levelScripts[currentLevel].allowedClowns.Count)];
        _clown = Instantiate(selectedClown).GetComponent<Clown>();
        _clown.onHit += ClownFound;

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
