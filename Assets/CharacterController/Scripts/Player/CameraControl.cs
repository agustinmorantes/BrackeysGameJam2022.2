using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private KeyCode moveCameraKey;
    
    private float current;
    private float target;
    private Vector3 targetRotation;
    [SerializeField] private Vector3 rotA, rotB;
    [SerializeField] private Controls controls;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float speed;
    void Start()
    {
        moveCameraKey = controls.moveCameraKey;
        targetRotation = rotA;
    }

    private void Update()
    {
        changeCameraRotation();
    }


    private void changeCameraRotation()
    {
        if (Input.GetKeyDown(moveCameraKey)) targetRotation = (targetRotation == rotA) ? rotB : rotA;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), curve.Evaluate(speed) *Time.deltaTime);
    }
}
