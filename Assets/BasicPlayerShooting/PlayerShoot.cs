using System.Collections;
using System.Collections.Generic;
using Bullets;
using UnityEngine;
using static BrackeysGameJam.Globals;

public class PlayerShoot : MonoBehaviour
{
    public BulletProperties bulletProperties;
    private Controls characterControlls;
    [SerializeField] private Transform playerAim;
    

    void Start()
    {
        characterControlls = GetComponent<Controls>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(playerAim.transform.position,playerAim.transform.forward * 500f,Color.magenta);
        if (Input.GetMouseButton(0))
            bulletSystem.Shoot(playerAim.transform.position,playerAim.transform.forward,bulletProperties);
    }
    
}
