using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSens;

    [HideInInspector] public Transform orientation;
    [HideInInspector] public Transform player;

    [HideInInspector] public Vector3 offset;

    [HideInInspector] public bool playerSpawned = false;
    
    float yRotation = 0; 
    float xRotation = 0; 
 
    void Update()
    {
        if (! playerSpawned) return;
        
        transform.parent.position = player.transform.position + offset;

        //camera
        //get value
        float mouseX = Input.GetAxisRaw("mouseX") * Time.deltaTime * cameraSens;
        float mouseY = Input.GetAxisRaw("mouseY") * Time.deltaTime * cameraSens;

        //math it
        yRotation += mouseX;
        xRotation -= mouseY;

        //clamp so don't go upsidedown
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
