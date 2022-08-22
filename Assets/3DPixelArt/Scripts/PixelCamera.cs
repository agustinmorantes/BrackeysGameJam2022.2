using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class PixelCamera : MonoBehaviour
{
    public RenderTexture cameraRT;
    public int resolutionRatio = 5;
    
    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        if (cameraRT != null) return;
        
        Debug.LogError("Please provide a render texture", this);
        enabled = false;
    }

    private void Start()
    {
        cameraRT.width = Screen.width / resolutionRatio;
        cameraRT.height = Screen.height / resolutionRatio;
        cameraRT.filterMode = FilterMode.Point;
        cameraRT.antiAliasing = 1;

        _cam.targetTexture = cameraRT;
    }
}
