using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEnemy : SimpleEnemy
{
    public int burstBulletCount = 20;
    public float waitAfterBurst = 5;

    public override IEnumerator ShootingCoroutine()
    {
        var wait = new WaitForSeconds(1f / firingRate);
        var burstWait = new WaitForSeconds(waitAfterBurst);

        yield return new WaitForSeconds(initialWait);

        while (true)
        {
            for (int i = 0; i < burstBulletCount; i++)
            {
                Shoot();
                yield return wait;
            }
            
            yield return burstWait;
        }
    }
}
