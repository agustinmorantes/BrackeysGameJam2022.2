using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalAmmo;
    [SerializeField] private TextMeshProUGUI ammoLeft;
    [SerializeField] private TextMeshProUGUI magAmmo; 
    public PlayerShoot player;


    private void LateUpdate()
    {
        totalAmmo.text = player.weaponProperties.maxAmmo.ToString();
        ammoLeft.text = player.ammoOnPlayerLeft.ToString();
        magAmmo.text = player.ammoOnMagLeft.ToString();

    }
}
