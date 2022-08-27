using UnityEngine;
using static BrackeysGameJam.Globals;

public class ShotgunEnemy : SimpleEnemy
{
    public int pellets = 3;
    public float angle = 30;
    
    public override void Shoot()
    {
        var t = transform;
        var spawnDist = Vector3.Distance(t.position, shootOrigin.position);
        
        for (var i = 0; i < pellets; i++)
        {
            var pelletAngle = (angle / (pellets-1)) * i - angle*0.5f;
            var dir = Quaternion.AngleAxis(pelletAngle, Vector3.up) * t.forward;
            var pos = t.position + dir * spawnDist;
            pos.y = shootOrigin.position.y;
            
            bulletSystem.Shoot(pos, dir, bulletProperties);
        }
    }
}
