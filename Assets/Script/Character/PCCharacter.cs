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

    [SerializeField]
    Transform flashlightTarget;
    [SerializeField]
    Transform flashlight;

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
        if(controller.enabled)
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

        }

        Debug.Log("X : " + Input.GetAxis("Mouse X"));
        Debug.Log("Y : " + Input.GetAxis("Mouse Y"));

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.localEulerAngles = new Vector3(0, yaw, 0.0f);
        cam.transform.localEulerAngles = new Vector3(pitch, 0, 0.0f);

        flashlight.LookAt(flashlightTarget);
    }

    public void AllignPlayer(Transform newTransform)
    {
        yaw = newTransform.localEulerAngles.y;
        pitch = newTransform.localEulerAngles.x;
    }
}