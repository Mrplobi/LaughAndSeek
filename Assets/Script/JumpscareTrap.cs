using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Jumpscare();
        }
    }
}
