using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    public delegate void HitEvent();
    public HitEvent onHit;

    [SerializeField]
    AudioSource darknessLaugh;
    [SerializeField]
    AudioSource trapLaugh;
    [SerializeField]
    List<AudioClip> trapLaughs;

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
        Destroy(gameObject);
    }

    public void PlayDarknessLaugh()
    {
        darknessLaugh.Play();
    }
    public void StopDarknessLaugh()
    {
        darknessLaugh.Stop();
    }

    public void PlayTrapLaugh()
    {
        if (trapLaugh.isPlaying) return;
        AudioClip clip = trapLaughs[Random.Range(0, trapLaughs.Count)];
        trapLaugh.clip = clip;
        trapLaugh.Play();
    }

    public void StopAllLaugh()
    {
        darknessLaugh.Stop();
        trapLaugh.Stop();
    }
}
