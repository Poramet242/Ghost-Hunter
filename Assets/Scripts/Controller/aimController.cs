using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimController : MonoBehaviour
{
    public static aimController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public bool isAim;
    [SerializeField] private float appropriate = 10f;
    [SerializeField] private Vector3 defPos;
    private Vector3 worldPosition;
    void Update()
    {
        if (isAim)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = appropriate; // Set an appropriate distance from the camera
            if (ARGameManager.instance.isAR)
            {
                worldPosition = ARGameManager.instance.ARCamera.ScreenToWorldPoint(mousePosition);
            }
            else
            {
                worldPosition = ARGameManager.instance.mainCamera.ScreenToWorldPoint(mousePosition);//Camera.main.ScreenToWorldPoint(mousePosition);
            }
            gameObject.transform.position = worldPosition;
        }
        if (ARGameManager.instance.isAR)
        {
            this.transform.LookAt(ARGameManager.instance.ARCamera.transform);
        }
        else
        {
            this.transform.LookAt(ARGameManager.instance.mainCamera.transform);
        }
    }
}
