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
        transform.SetParent(spawnGO.transform);
        transform.localPosition = spawn.spawnOffset;
        transform.localScale = spawn.spawnScale;
    }
    public void Hit()
    {
        gameObject.SetActive(false);
        onHit.Invoke();
        Destroy(this.gameObject);
    }
}
