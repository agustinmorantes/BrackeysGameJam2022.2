using System;
using System.Collections;
using System.Collections.Generic;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public class PlayerShoot : MonoBehaviour
{
    public BulletProperties bulletProperties;
    [SerializeField] private Transform playerAim;
    private Controls controls;
    
    // Update is called once per frame
    private void Start()
    {
        controls = GetComponent<Controls>();
    }

    void Update()
    {
        Debug.DrawLine(playerAim.transform.position,playerAim.transform.forward * 500f,Color.magenta);
        if (Input.GetMouseButton(0))
            bulletSystem.Shoot(playerAim.transform.position,playerAim.transform.forward,bulletProperties);
    }
    
    
}
