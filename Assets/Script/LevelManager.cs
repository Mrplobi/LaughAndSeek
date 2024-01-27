using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject clownPrefab;
    Clown _clown;

    [SerializeField]
    List<LevelScriptable> levelScripts;

    [SerializeField]
    Transform playerStart;
    [SerializeField]
    Player playerObject;

    int currentLevel = 0;

    private void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        if (currentLevel >= levelScripts.Count)
            return;

        playerObject.transform.position = playerStart.position;
        playerObject.transform.rotation = playerStart.rotation;

        if(_clown == null)
        {
            _clown = Instantiate(clownPrefab).GetComponent<Clown>();
            _clown.onHit += ClownFound;
        }

        var spawn = levelScripts[currentLevel].GetRandomSpawn();
        SpawnClown(spawn);
    }

    public void SpawnClown(SpawnInfo spawn)
    {
        _clown.Spawn(spawn);
    }

    public void ClownFound()
    {
        currentLevel++;
        StartRound();
    }

    public void OnFlashlightChanged(bool isOn)
    {
        if (isOn)
        {
            PlayLaugh();
        }
        else
        {

        }
    }

    private void PlayLaugh()
    {
        _clown.PlayLaugh();
    }
}
