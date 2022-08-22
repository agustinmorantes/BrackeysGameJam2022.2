using System;
using System.Collections;
using System.Collections.Generic;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public class BulletTester : MonoBehaviour
{
    public BulletProperties bulletProperties;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bulletSystem.Shoot(_transform.position, _transform.forward, bulletProperties);
        }
    }
}
