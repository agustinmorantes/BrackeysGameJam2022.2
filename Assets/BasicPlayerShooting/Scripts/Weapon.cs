using System.Collections;
using System.Collections.Generic;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public abstract class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public WeaponProperties weaponProperties;
    public Transform muzzle;
    
    protected bool canShoot=true;
    
    private int ammoOnPlayer;
    private int ammoOnMag;
    public abstract void Shoot();

    public void Reload()
    {
        ammoOnPlayer -= weaponProperties.bulletsPerMag;
        ammoOnMag = weaponProperties.bulletsPerMag;
    }

    protected void ResetShoot()
    {
        canShoot = true;
    }
}
