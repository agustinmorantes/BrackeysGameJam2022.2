using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLocker : MonoBehaviour
{
    private bool isLocked;
    // Start is called before the first frame update
    void Start()
    {
        ChangeScreenState(false,CursorLockMode.Locked);
    }

    private void ChangeScreenState(bool visible,CursorLockMode mode)
    {
        Cursor.lockState = mode;
        Cursor.visible = visible;
        isLocked = !visible;
    }

    private bool isScreenLocked()
    {
        return isLocked;
    }
}