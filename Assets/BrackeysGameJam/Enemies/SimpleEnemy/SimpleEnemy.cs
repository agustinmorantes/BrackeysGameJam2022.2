using System;
using System.Collections;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public class SimpleEnemy : MonoBehaviour
{
    public BulletProperties bulletProperties;
    public float firingRate = 2;
    
    private IEnumerator ShootState()
    {
        while (true)
        {
            var t = transform;
            var dir = t.forward;
            var pos = t.position + dir * 1f + Vector3.up * 1f;
            bulletSystem.Shoot(pos, dir, bulletProperties);
            yield return new WaitForSeconds(1.0f / firingRate);
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
