using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [Header("Character Movement")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    [Header("Camera Controls")]
    public KeyCode moveCameraKey = KeyCode.Q;

    [Header("Character Actions")] 
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode interactKey = KeyCode.E;
//    public bool shootKey = Input.GetMouseButton(0);
}
