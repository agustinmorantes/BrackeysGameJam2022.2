using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDash : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerCam;
    private Rigidbody rb;
    private CharacterController characterController;
    private Controls playerControls;

    [Header("Dash")] 
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float dashDuration;

    [Header("Cooldown")] 
    [SerializeField] private float dashCooldown;
    private float coolDownTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerControls = GetComponent<Controls>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(playerControls.dashKey)) Dash();
        DrawDashVector();
    }

    private void Dash()
    {
        Vector3 forceToApply = characterController.GetMoveDir().normalized * dashForce;
        rb.AddForce(forceToApply,ForceMode.Impulse);

        Invoke(nameof(ResetDash),dashDuration);
    }

    private void DrawDashVector()
    {
        Vector3 forceToApply = characterController.GetMoveDir().normalized * dashForce;
        Debug.DrawLine(orientation.position,forceToApply * 5000.0f,Color.red); 
    }

    private void ResetDash()
    {
        
    }
}