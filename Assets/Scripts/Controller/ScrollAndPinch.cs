using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{
    [Header("Camera")]
    public float distance = 60.0f;   // Distance from the player
    public float rotationSpeed = 10.0f;
    public float rotationSmoothness = 50.0f; // Smoothness factor for rotation
    public Transform player;        // Reference to the player GameObject
    private Vector3 offset;
    //--------------------------------------------------------------------------------
    [Header("Zoom")]
    public float zoomSpeed = 5.0f;
    public float minDistance = 30.0f;
    public float maxDistance = 70.0f;
    private float initialDistance;
    private void Start()
    {
        offset = transform.position - player.position;
    }

#if UNITY_EDITOR
     private void LateUpdate()
     {
         // Rotate the camera around the player based on swipe input
         if (Input.GetMouseButton(0))
         {
             float swipeX = Input.GetAxis("Mouse X");
             float desiredRotation = swipeX * rotationSpeed;
             Quaternion rotations = Quaternion.Euler(50f, desiredRotation, 0f);
             Quaternion newRotation = transform.rotation * rotations;
             transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSmoothness * Time.deltaTime);
         }

         // Calculate the desired camera position based on the new rotation
         Quaternion rotation = Quaternion.Euler(50f, transform.eulerAngles.y, 0f);
         Vector3 desiredPosition = player.position - (rotation * Vector3.forward) * distance;

         // Update the camera position and rotation
         transform.position = desiredPosition;
         transform.LookAt(player);

     }
#endif

#if !UNITY_EDITOR
private void LateUpdate()
{
    // Handle camera rotation based on touch input
    if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
    {
        Touch touch = Input.GetTouch(0);

        float swipeX = touch.deltaPosition.x;
        float desiredRotation = swipeX * rotationSpeed;
        Quaternion rotations = Quaternion.Euler(50f, desiredRotation, 0f);
        Quaternion newRotation = transform.rotation * rotations;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSmoothness * Time.deltaTime);
    }

    // Handle camera zoom based on touch input
    if (Input.touchCount == 2)
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        if (touch2.phase == TouchPhase.Began)
        {
            initialDistance = Vector2.Distance(touch1.position, touch2.position);
        }
        else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
        {
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float deltaDistance = initialDistance - currentDistance;
            float zoomAmount = deltaDistance * zoomSpeed * Time.deltaTime;
            float newDistance = Mathf.Clamp(distance + zoomAmount, minDistance, maxDistance);
            distance = newDistance;
        }
    }

    // Calculate the desired camera position based on the new rotation and distance
    Quaternion rotation = Quaternion.Euler(50f, transform.eulerAngles.y, 0f);
    Vector3 desiredPosition = player.position - (rotation * Vector3.forward) * distance;

    // Update the camera position and rotation
    transform.position = desiredPosition;
    transform.LookAt(player);
}
#endif
}
