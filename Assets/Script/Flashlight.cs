using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    GameObject lightCone;
    [SerializeField]
    AudioSource clickSound;
    [SerializeField]
    Transform RaycastStart;

    bool isActive = true;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            isActive = !isActive;
            lightCone.SetActive(isActive);
            clickSound.Play();
        }

        if (!isActive) return;
        
        RaycastHit hit;
        Physics.Raycast(RaycastStart.position, RaycastStart.forward, out hit);
        Debug.DrawLine(RaycastStart.position, RaycastStart.position + RaycastStart.forward * 10, Color.magenta, 0.1f);
        if (hit.collider != null && hit.collider.tag == "Clown")
        {
            Debug.Log(hit.collider.gameObject.name);
            hit.collider.GetComponentInParent<Clown>().Hit();
        }
    }
}
