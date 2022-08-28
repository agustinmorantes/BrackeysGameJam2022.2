using System;
using System.Collections;
using Bullets;
using Lazlo;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using static BrackeysGameJam.Globals;

public class SimpleEnemy : MonoBehaviour
{
    public BulletProperties bulletProperties;
    public float firingRate = 2;
    public float shootingDistance = 5;
    public float aimingAngularSpeed = 180;
    public Transform shootOrigin;
    public float lineOfSightRadius = 0.25f;
    public LayerMask lineOfSightMask;
    public float initialWait = 0.25f;
    public float initialDetectionRange = 10;

    [SerializeField]
    private Dependency<NavMeshAgent> _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent.Resolve(this);

    public virtual void Shoot()
    {
        var t = transform;
        var dir = t.forward;
        var pos = shootOrigin.position;
        bulletSystem.Shoot(pos, dir, bulletProperties);
    }

    public virtual bool IsPlayerInDetectionRange()
    {
        var dist = Vector3.Distance(player.transform.position, transform.position);
        return dist < initialDetectionRange;
    }
    
    public virtual IEnumerator ShootingCoroutine()
    {
        var wait = new WaitForSeconds(1f / firingRate);

        yield return new WaitForSeconds(initialWait);

        while (true)
        {
            Shoot();
            
            yield return wait;
        }
    }

    public void Aim()
    {
        var t = transform;

        var targetRot = new Quaternion();
        var lookDir = (player.transform.position - t.position).normalized;
        targetRot.SetLookRotation(lookDir, Vector3.up);
        var euler = targetRot.eulerAngles;
        targetRot.eulerAngles = new(0, euler.y, euler.z);
        
        t.rotation = Quaternion.RotateTowards(t.rotation, targetRot, aimingAngularSpeed * Time.deltaTime);
    }

    public bool CanShoot()
    {
        var pos = shootOrigin.position;
        var playerPos = player.transform.position;
        playerPos.y = pos.y;
        var playerDir = (playerPos - pos).normalized;
        
        var dist = Vector3.Distance(pos, playerPos);
        
        var ray = new Ray(pos, playerDir);
        var hasLineOfSight = !Physics.SphereCast(ray, lineOfSightRadius, out var hitInfo, dist, lineOfSightMask) || hitInfo.transform.CompareTag("Player");
        
        return dist < shootingDistance && hasLineOfSight;
    }

    public void StartWalking()
    {
        NavMeshAgent.destination = player.transform.position;
        NavMeshAgent.isStopped = false;
    }
    
    public void WalkTowardsPlayer()
    {
        NavMeshAgent.destination = player.transform.position;
    }

    public void StopWalking()
    {
        NavMeshAgent.isStopped = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || !Application.isEditor) return;
        
        var pos = shootOrigin.position;
        var playerPos = player.transform.position;
        playerPos.y = pos.y;
        var playerDir = (playerPos - pos).normalized;
        
        var dist = Vector3.Distance(pos, playerPos);
        
        var ray = new Ray(pos, playerDir);
        var hasLineOfSight = !Physics.SphereCast(ray, lineOfSightRadius, out var hitInfo, dist, lineOfSightMask) || hitInfo.transform.CompareTag("Player");

        var drawDist = hasLineOfSight ? dist : hitInfo.distance;

        Gizmos.color = hasLineOfSight ? Color.green : Color.red;
        Gizmos.DrawRay(pos, playerDir * drawDist);
    }
}
