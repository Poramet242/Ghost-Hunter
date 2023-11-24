using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStretch : MonoBehaviour
{
    private Vector2 screenResolution;
    private void Start()
    {
        screenResolution = new Vector2(Screen.width, Screen.height);
        MatchPlanteToScreenSize();
    }
    private void Update()
    {
        if (screenResolution.x != Screen.width || screenResolution.y != Screen.height)
        {
            MatchPlanteToScreenSize();
            screenResolution.x = Screen.width;
            screenResolution.y = Screen.height;
        }
    }
    public void MatchPlanteToScreenSize()
    {
        float planeToCameradistance = Vector3.Distance(gameObject.transform.position,Camera.main.transform.position);
        float planeHeighScale = (2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * planeToCameradistance / 10.0f);
        float planeWidthScale = planeHeighScale * Camera.main.aspect;
        gameObject.transform.localScale = new Vector3(planeWidthScale, 1, planeHeighScale);
    }
}
