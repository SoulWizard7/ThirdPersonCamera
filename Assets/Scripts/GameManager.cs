using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [NonSerialized]public bool isAimingCameraMode = false;
    public bool invertMouseLook = false;
    public bool lockCursorAtStart = false;
    public float aimZoom = 4;

    [Header("EXPERIMENTAL AIM OFFSET SYSTEM, UNDER DEVELOPMENT!!!")]
    public bool useAimOffsetSystem;
    public float cameraOffsetModifier = 1;
    public float cameraToAimingLerpValue = 0.5f;
    
    void Start()
    {
        if (lockCursorAtStart) { Cursor.lockState = CursorLockMode.Locked; }
    }
}
