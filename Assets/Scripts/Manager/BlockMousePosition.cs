using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMousePosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Aim")
        {
            Debug.Log("Aim !!");
            //aimController.instance.isAim = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Aim")
        {
            Debug.Log("out Aim !!");
            //aimController.instance.isAim = false;
        }
    }
}
