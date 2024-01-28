using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Whoopie : Trap
{
    AudioSource source;
    Collider detectionCollider;

    public UnityEvent onCollide;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        detectionCollider = GetComponent<Collider>();
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
        DeActivate();
    }

    public override void Activate()
    {
        detectionCollider.enabled = true;
        GetComponentInParent<MeshRenderer>().enabled = true;
    }

    public override void DeActivate()
    {
        detectionCollider.enabled = false;
    }
    public override void Hide()
    {
        DeActivate();
        GetComponentInParent<MeshRenderer>().enabled = false;
    }
}
