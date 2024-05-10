using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Vivox;


public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    [SerializeField] Vector3 offset;
    [SerializeField] VoiceChat vc;

    public override void OnNetworkSpawn()
    {
        if (! IsOwner) return;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject playerCamera = GameObject.Find("Camera");
        
        playerCamera.GetComponent<CameraController>().orientation = orientation;
        playerCamera.GetComponent<CameraController>().playerSpawned = true;

        playerCamera.GetComponent<CameraController>().player = transform;
        playerCamera.GetComponent<CameraController>().offset = offset;
    }

    void Update()
    {
        if (! IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce * 100);
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
        Vector3 moveDir = new Vector3(0, 0, 0);

        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        moveDir = orientation.forward * vInput + orientation.right * hInput;

        // moveDir 
        rb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime * 10f, ForceMode.Force);

        Vector3 clampVel = rb.velocity;
        clampVel.x = Mathf.Clamp(clampVel.x, maxSpeed * -1, maxSpeed);
        clampVel.z = Mathf.Clamp(clampVel.z, maxSpeed * -1, maxSpeed);
        rb.velocity = clampVel;
    }
}