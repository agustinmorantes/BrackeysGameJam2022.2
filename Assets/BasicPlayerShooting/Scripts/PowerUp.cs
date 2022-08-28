using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public WeaponProperties powerUpProp;
    public PlayerShoot shoot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            PickUp(other);
        }
    }

    private void PickUp(Collider player)
    {
        shoot = player.transform.root.GetComponent<PlayerShoot>();
        shoot.weaponProperties = powerUpProp;
        shoot.ammoOnPlayerLeft = powerUpProp.maxAmmo;
        shoot.ammoOnMagLeft = powerUpProp.bulletsPerMag;
        Destroy(gameObject);
    }
}
