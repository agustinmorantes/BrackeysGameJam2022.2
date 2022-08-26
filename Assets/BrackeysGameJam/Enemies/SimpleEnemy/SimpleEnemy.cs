using System;
using System.Collections;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public class SimpleEnemy : MonoBehaviour
{
    public BulletProperties bulletProperties;
    public float firingRate = 2;

    public IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            var t = transform;
            var dir = t.forward;
            var pos = t.position + dir * 1f + Vector3.up * 1f;
            bulletSystem.Shoot(pos, dir, bulletProperties);

            yield return new WaitForSeconds(1f / firingRate);
        }
    }

    public void Aim()
    {
        var t = transform;
        t.LookAt(player.transform.position);
        
        var rot = t.rotation;
        var euler = rot.eulerAngles;
        rot.eulerAngles = new(0, euler.y, euler.z);
        t.rotation = rot;
    }
}
