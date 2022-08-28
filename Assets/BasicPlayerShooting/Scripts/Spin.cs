using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 20;

        // Update is called once per frame
    void Update()
    {
       transform.Rotate(0,0,turnSpeed * Time.deltaTime,Space.Self); 
    }
}