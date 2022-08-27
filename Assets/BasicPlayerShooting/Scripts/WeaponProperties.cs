using System.Collections;
using System.Collections.Generic;
using Bullets;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Properties", fileName = "New Weapon Type")]
public class WeaponProperties : ScriptableObject
{
    public float rateOfFire;
    public int maxAmmo;
    public int ammoConsumedPerShot;
    public int bulletsPerMag;
    public BulletProperties bulletProperties;
}
