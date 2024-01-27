using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    private bool isSoundPlaying = false;
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private AudioSource audioSourceFootsteps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(characterController.velocity.magnitude);
        if (characterController.velocity.magnitude >= 0.05f && isSoundPlaying==false)
        {

            Debug.Log("player is moving");
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
