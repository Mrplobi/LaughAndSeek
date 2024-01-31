using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrap : Trap
{
    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public override void Hide()
    {
        DeActivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Entered Collision : " + other.name);
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player != null)
        {
            player.Jumpscare();
        }
        DeActivate();
    }
}
