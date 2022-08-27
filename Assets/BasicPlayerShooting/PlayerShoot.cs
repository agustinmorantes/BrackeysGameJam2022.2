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
    public int ammoOnMagLeft;
    public int ammoOnPlayerLeft;
    
        // Update is called once per frame
    private void Start()
    {
        controls = GetComponent<Controls>();
        ammoOnPlayerLeft = weaponProperties.maxAmmo;
        ammoOnMagLeft = weaponProperties.bulletsPerMag;
    }

    void Update()
    {
        Debug.DrawLine(playerAim.transform.position,playerAim.transform.forward * 500f,Color.magenta);
        if (Input.GetMouseButton(0) && canShoot && ammoOnMagLeft > 0)
        {
            canShoot = false;
            ammoOnMagLeft -= weaponProperties.ammoConsumedPerShot;
            bulletSystem.Shoot(playerAim.transform.position,playerAim.transform.forward,weaponProperties.bulletProperties);
            Invoke(nameof(ResetShoot),weaponProperties.rateOfFire);
        }
        if (Input.GetKeyDown(controls.reloadKey))
        {
           if (ammoOnPlayerLeft < weaponProperties.bulletsPerMag) Reload(ammoOnPlayerLeft);
           else Reload(weaponProperties.bulletsPerMag);
               
        }

    }

    private void Reload(int amount)
    {
        ammoOnMagLeft += amount;
        ammoOnPlayerLeft -= amount;
    }

    private void ResetShoot()
    {
        canShoot = true;
    }

}
