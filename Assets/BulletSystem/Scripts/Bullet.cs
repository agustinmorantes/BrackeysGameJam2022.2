using System;
using System.Collections;
using System.Collections.Generic;
using BrackeysGameJam;
using Bullets;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject afterHitParticlesPrefab;
    
    public BulletProperties Properties { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        var entityHealth = other.GetComponentInParent<Health>();
        if (entityHealth)
        {
            entityHealth.Damage(Properties.damage);
        }

        if(afterHitParticlesPrefab) Instantiate(afterHitParticlesPrefab);
        
        Destroy(gameObject);
    }
}
