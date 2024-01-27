using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    public delegate void HitEvent();
    public HitEvent onHit;

    [SerializeField]
    Vector3 spawnOffset = Vector3.zero;
    public void Spawn(Transform spawnPosition)
    {
        transform.SetParent(spawnPosition);
        transform.localPosition = spawnOffset;
    }
    public void Hit()
    {
        gameObject.SetActive(false);
        onHit.Invoke();
        Destroy(this.gameObject);
    }
}
