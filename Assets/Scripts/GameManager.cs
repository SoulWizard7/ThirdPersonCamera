using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [NonSerialized]public bool isAimingCameraMode = false;
    public bool invertMouseLook = false;
    public bool lockCursorAtStart = false;
    void Start()
    {
        if (lockCursorAtStart) { Cursor.lockState = CursorLockMode.Locked; }
    }
    private void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            isAimingCameraMode = true;
        }
        else
        {
            isAimingCameraMode = false;
        }
    }
}
