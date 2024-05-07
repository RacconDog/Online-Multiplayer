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

    [SerializeField] Vector3 offset;
    [SerializeField] VoiceChat vc;

    public override void OnNetworkSpawn()
    {
        if (! IsOwner) return;


        GameObject playerCamera = GameObject.Find("Camera");
        
        playerCamera.GetComponent<CameraController>().orientation = orientation;
        playerCamera.GetComponent<CameraController>().playerSpawned = true;

        playerCamera.GetComponent<CameraController>().player = transform;
        playerCamera.GetComponent<CameraController>().offset = offset;
    }

    void Update()
    {
        if (! IsOwner) return;

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
    }
}