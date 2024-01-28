using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Entered Collision");
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player != null)
        {
            player.Jumpscare();
        }
    }
}
