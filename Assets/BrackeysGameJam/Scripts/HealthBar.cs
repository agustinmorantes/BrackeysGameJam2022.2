using System.Collections;
using System.Collections.Generic;
using BrackeysGameJam;
using UnityEngine;
using UnityEngine.UI;
using static BrackeysGameJam.Globals;

public class HealthBar : MonoBehaviour
{
    public void OnHealthChanged()
    {
        var health = player.GetComponent<Health>();
        GetComponent<Slider>().value = health.CurrentHP / health.maxHP;
    }
}
