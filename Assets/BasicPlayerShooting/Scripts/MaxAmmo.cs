using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            GiveAmmo(other);
        }
    }

    private void GiveAmmo(Collider player)
    {
        Debug.Log("Should Give me loot");
        var playerAmmunition = player.transform.root.GetComponent<PlayerShoot>();
        playerAmmunition.ammoOnPlayerLeft = playerAmmunition.weaponProperties.maxAmmo;
        Destroy(gameObject);
    }
}
