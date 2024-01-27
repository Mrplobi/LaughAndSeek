using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    public delegate void HitEvent();
    public HitEvent onHit;

    public void Spawn(SpawnInfo spawn)
    {
        var spawnGO = GameObject.Find(spawn.spawnName);
        transform.SetParent(spawnGO.transform, false);
        transform.localPosition = spawn.spawnOffset;
        transform.localScale = spawn.spawnScale;
    }
    public void Hit()
    {
        onHit.Invoke();
    }
}
