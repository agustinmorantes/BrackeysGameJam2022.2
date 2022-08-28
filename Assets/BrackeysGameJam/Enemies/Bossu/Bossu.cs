using System.Collections;
using Bullets;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BrackeysGameJam.Globals;

public class Bossu : MonoBehaviour
{
    [Header("Movement")]
    public float noiseScale = 0.2f;
    public Transform leftBound;
    public Transform rightBound;
    
    [Header("Boss Phases")] public float initialWait = 3.0f;
    public float waitBetweenPhases = 3.0f;

    [Header("Attack 1")] public int attack1BulletRingCount = 10;
    public float attack1WaitBetweenRings = 0.5f;
    public float attack1RingConeAngle = 90f;
    public int attack1PelletsPerRing = 50;

    [Header("Attack 2")]
    public int attack2BurstBulletCount = 100;
    public float attack2WaitBetweenBursts = 5.0f;
    public float attack2FireRate = 10;

    [Header("Attack 3")]
    public int attack3ShotCount = 5;
    public float attack3WaitBetweenShotsAvg = 2.0f;
    public float attack3WaitBetweenShotsDev = 0.5f;
    public BulletProperties attack3BulletProperties;

    [Header("Etc.")]
    public BulletProperties bulletProperties;
    public Transform shotOrigin;

    public IEnumerator Attack1Shoot()
    {
        var waitBetweenRings = new WaitForSeconds(attack1WaitBetweenRings);

        var t = transform;
        var spawnDist = Vector3.Distance(t.position, shotOrigin.position);

        for (var ring = 0; ring < attack1BulletRingCount; ring++)
        {
            for (var i = 0; i < attack1PelletsPerRing; i++)
            {
                if (i % 2 == ring % 2) continue;

                var pelletAngle = (attack1RingConeAngle / (attack1PelletsPerRing - 1)) * i -
                                  attack1RingConeAngle * 0.5f;
                var dir = Quaternion.AngleAxis(pelletAngle, Vector3.up) * t.forward;
                var pos = t.position + dir * spawnDist;
                pos.y = shotOrigin.position.y;

                bulletSystem.Shoot(pos, dir, bulletProperties);
            }

            yield return waitBetweenRings;
        }
    }

    public IEnumerator Attack2Shoot()
    {
        var wait = new WaitForSeconds(1f / attack2FireRate);
        var burstWait = new WaitForSeconds(attack2WaitBetweenBursts);

        var t = transform;
        var spawnDist = Vector3.Distance(t.position, shotOrigin.position);

        void Shoot(int i)
        {
            var pelletAngle = (attack1RingConeAngle / (attack2BurstBulletCount - 1)) * i - attack1RingConeAngle * 0.5f;
            var dir = Quaternion.AngleAxis(pelletAngle, Vector3.up) * t.forward;
            var pos = t.position + dir * spawnDist;
            pos.y = shotOrigin.position.y;
            bulletSystem.Shoot(pos, dir, bulletProperties);
        }

        for (var i = 0; i < attack2BurstBulletCount; i++)
        {
            Shoot(i);
            yield return wait;
        }

        yield return burstWait;

        for (var i = 0; i < attack2BurstBulletCount; i++)
        {
            Shoot(attack2BurstBulletCount - i - 1);
            yield return wait;
        }
    }

    public IEnumerator Attack3Shoot()
    {
        for (var i = 0; i < attack3ShotCount; i++)
        {
            var playerPos = player.transform.position;

            var pos = shotOrigin.position; 
            var dir = (playerPos - pos).normalized;
            dir.y = 0;
            
            bulletSystem.Shoot(pos, dir, attack3BulletProperties);
            
            yield return new WaitForSeconds(Random.Range(attack3WaitBetweenShotsAvg - attack3WaitBetweenShotsDev,
                attack3WaitBetweenShotsAvg + attack3WaitBetweenShotsDev));
        }
    }

    private IEnumerator Start()
    {
        Random.InitState((int)Time.realtimeSinceStartupAsDouble * 1024);
        var wait = new WaitForSeconds(waitBetweenPhases);
        
        while (true)
        {
            var attack = Random.Range(1, 4);

            switch (attack)
            {
                case 1:
                    yield return Attack1Shoot();
                    break;
                case 2:
                    yield return Attack2Shoot();
                    break;
                case 3:
                    yield return Attack3Shoot();
                    break;
            }
            
            yield return wait;
        }
    }

    private void Update()
    {
        var noise = Mathf.Clamp01(Mathf.PerlinNoise(Time.time * noiseScale, 0));
        var posX = Mathf.Lerp(leftBound.position.x, rightBound.position.x, noise);

        var t = transform;
        t.position = new(posX, t.position.y, t.position.z);
    }

    public void OnBossDeath()
    {
        SceneManager.LoadScene(2);
    }
}