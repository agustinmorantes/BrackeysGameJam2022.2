using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInRoom;
    private void Update()
    {
        bool enemiesAlive = false;
        foreach (var enemy in enemiesInRoom)
        {
            if (enemy != null) enemiesAlive = true;
        }
        if (!enemiesAlive) Destroy(gameObject);
    }
}
