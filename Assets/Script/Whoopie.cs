using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Whoopie : MonoBehaviour
{
    AudioSource source;

    public UnityEvent onCollide;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (source != null)
        {
            source.Play();
        }
        if(onCollide != null)
        {
            onCollide.Invoke();
        }
    }
}
