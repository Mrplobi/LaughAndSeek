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
    [SerializeField]
    AudioClip onSound;
    [SerializeField]
    AudioClip offSound;

    bool isActive = true;

    bool hittingClown = false;

    [SerializeField]
    float validateTime = 0.5f;
    float validateTimer = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) || OVRInput.GetDown(OVRInput.Button.One))
        {
            isActive = !isActive;
            lightCone.SetActive(isActive);
            clickSound.clip = isActive? onSound : offSound;
            clickSound.Play();
        }

        if (!isActive) return;
        
        RaycastHit hit;
        Physics.Raycast(RaycastStart.position, RaycastStart.forward, out hit);
        Debug.DrawLine(RaycastStart.position, RaycastStart.position + RaycastStart.forward * 10, Color.magenta, 0.1f);
        if (hit.collider != null && hit.collider.tag == "Clown")
        {
            hittingClown = true;
        }
        else
        {
            hittingClown = false;
        }

        if(hittingClown)
        {
            validateTimer += Time.deltaTime;
            Debug.LogWarning(validateTimer);
            if (validateTimer > validateTime)
            {
                Debug.LogWarning(validateTimer);
                hit.collider.GetComponentInParent<Clown>().Hit();
            }
        }
        else
        {
            validateTimer = 0;
        }
    }
}
