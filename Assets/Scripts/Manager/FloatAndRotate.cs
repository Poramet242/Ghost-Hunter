using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [SerializeField] private float rotatespeed = 50f;
    [SerializeField] private float floatAmplitude = 2.0f;
    [SerializeField] private float floatFrequency = 0.5f;
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, rotatespeed * Time.deltaTime);
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
        transform.position = tempPos;
    }
}
