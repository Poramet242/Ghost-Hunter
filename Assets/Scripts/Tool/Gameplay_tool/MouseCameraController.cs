using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseCameraController : MonoBehaviour
{
    public static MouseCameraController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
#if UNITY_EDITOR
    public Transform player;
    public float scrollSpeed = 5.0f;
    public float minDistance = 5.0f;
    public float maxDistance = 20.0f;
    public float zoomSpeed = 0.5f;
    public float rotationSpeed = 5.0f;
    public float rotationSmoothness = 10.0f;

    private Vector3 offset;

    private void Start()
    {
        //offset = transform.position - player.position;
        offset = transform.position * maxDistance;
        LoadSavedPos();
    }
    private void Update()
    {

    }
    private void LateUpdate()
    {
        // Scroll to move
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scroll * zoomSpeed;
        float newDistance = Mathf.Clamp(offset.magnitude - zoomAmount, minDistance, maxDistance);
        offset = offset.normalized * newDistance;

        // Rotate the camera around the player
        if (Input.GetMouseButton(0))
        {
            float swipeX = Input.GetAxis("Mouse X");
            float desiredRotation = swipeX * rotationSpeed;
            Quaternion rotation = Quaternion.Euler(0f, desiredRotation, 0f);
            offset = rotation * offset;
        }

        // Update camera position and rotation
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * scrollSpeed);
        transform.LookAt(player);

        // Save position, rotation, and offset
        /*if (Input.GetKeyDown(KeyCode.S))
        {
            savedPosition = transform.position;
            savedRotation = transform.rotation;
            savedOffset = offset;
            Debug.Log("Save position and rotation.");
        }*/

        // Load saved position, rotation, and offset
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = savedPosition;
            transform.rotation = savedRotation;
            offset = savedOffset;
            Debug.Log("Load position and rotation.");
        }*/
    }
    public void LoadSavedPos()
    {
        if (PlayerData.instance.savedPosition == Vector3.zero || PlayerData.instance.savedOffset == Vector3.zero)
        {
            return;
        }
        transform.position = PlayerData.instance.savedPosition;
        transform.rotation = PlayerData.instance.savedRotation;
        offset = PlayerData.instance.savedOffset;
        Debug.Log("Load position and rotation.");
    }
    public void SavePos()
    {
        PlayerData.instance.savedPosition = transform.position;
        PlayerData.instance.savedRotation = transform.rotation;
        PlayerData.instance.savedOffset = offset;
        Debug.Log("Save position and rotation.");

    }
#endif

#if !UNITY_EDITOR
    public Transform player;
    public float scrollSpeed = 5.0f;
    public float minDistance = 5.0f;
    public float maxDistance = 20.0f;
    public float zoomSpeed = 0.5f;
    public float rotationSpeed = 5.0f;

    private Vector3 offset;
    private float initialDistance;
    private Quaternion initialRotation;

    private void Start()
    {
        rotationSpeed = 0.05f;
        minDistance = 60.0f;
        maxDistance = 160.0f;
        initialRotation = transform.rotation;
        offset = -transform.forward * maxDistance;
        LoadSavedPos();
    }

    private void LateUpdate()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Rotate the camera around the player
                float swipeX = touch.deltaPosition.x;
                float desiredRotation = swipeX * rotationSpeed;
                Quaternion rotation = Quaternion.Euler(0f, desiredRotation, 0f);
                offset = rotation * offset;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                // Pinch to zoom
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float deltaDistance = currentDistance - initialDistance;
                float zoomAmount = deltaDistance * zoomSpeed;
                float newDistance = Mathf.Clamp(offset.magnitude - zoomAmount, minDistance, maxDistance);
                offset = offset.normalized * newDistance;
            }
        }

        // Update camera position and rotation
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * scrollSpeed);
        transform.LookAt(player);
    }
    public void LoadSavedPos()
    {
        if (PlayerData.instance.savedPosition == Vector3.zero || PlayerData.instance.savedOffset == Vector3.zero)
        {
            return;
        }
        transform.position = PlayerData.instance.savedPosition;
        transform.rotation = PlayerData.instance.savedRotation;
        offset = PlayerData.instance.savedOffset;
        Debug.Log("Load position and rotation.");
    }
    public void SavePos()
    {
        PlayerData.instance.savedPosition = transform.position;
        PlayerData.instance.savedRotation = transform.rotation;
        PlayerData.instance.savedOffset = offset;
        Debug.Log("Save position and rotation.");
    }
#endif
}
