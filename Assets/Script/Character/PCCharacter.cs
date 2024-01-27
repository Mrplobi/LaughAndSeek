using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCharacter : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private Camera cam; 
    private float speedH = 2.0f;
    private float speedV = 2.0f;

    private float yaw = 5f;
    private float pitch = 5f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if(controller  == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
        cam = Camera.main;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        var projectedForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;

        Vector3 move = projectedForward * Input.GetAxis("Vertical") + cam.transform.right.normalized * Input.GetAxis("Horizontal");        
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        cam.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}