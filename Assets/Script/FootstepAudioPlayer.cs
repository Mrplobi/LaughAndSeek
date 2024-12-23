using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioPlayer : MonoBehaviour
{

    private bool isSoundPlaying = false;
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private AudioSource audioSourceFootsteps;

    void Update()
    {
        if(!characterController.enabled && isSoundPlaying)
        {
            audioSourceFootsteps.Stop();
            isSoundPlaying = false;
        }
        if (characterController.velocity.magnitude >= 0.2f && isSoundPlaying==false)
        {
            audioSourceFootsteps.Play();
            isSoundPlaying = true;
        }
        if (characterController.velocity.magnitude <= 0.2f && isSoundPlaying == true)
        {
            audioSourceFootsteps.Stop();
            isSoundPlaying = false;
        }
    }
}
