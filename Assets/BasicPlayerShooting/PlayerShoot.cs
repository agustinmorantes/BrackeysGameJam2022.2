using System;
using System.Collections;
using System.Collections.Generic;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public class PlayerShoot : MonoBehaviour
{
    //public BulletProperties bulletProperties;
    [SerializeField] private Transform playerAim;
    private Controls controls;
    private bool canShoot = true;
    public WeaponProperties weaponProperties;
    [Header("Weapon System")] 
    private int ammoOnMagLeft;
    private int ammoOnPlayerLeft;
    
    
    

        // Update is called once per frame
    private void Start()
    {
        controls = GetComponent<Controls>();
    }

    void Update()
    {
        Debug.DrawLine(playerAim.transform.position,playerAim.transform.forward * 500f,Color.magenta);
        if (Input.GetMouseButton(0) && canShoot)
        {
            canShoot = false;
            bulletSystem.Shoot(playerAim.transform.position,playerAim.transform.forward,weaponProperties.bulletProperties);
            Invoke(nameof(ResetShoot),weaponProperties.rateOfFire);
        }

    }

    private void Shoot()
    {
        
    }

    private void ResetShoot()
    {
        canShoot = true;
    }

}
