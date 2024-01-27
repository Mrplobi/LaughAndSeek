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
        if (characterController.velocity.magnitude >= 0.05f && isSoundPlaying==false)
        {
            audioSourceFootsteps.Play();
            isSoundPlaying = true;
        }
        if (characterController.velocity.magnitude <= 0.05f && isSoundPlaying == true)
        {
            audioSourceFootsteps.Stop();
            isSoundPlaying = false;
        }
    }
}
