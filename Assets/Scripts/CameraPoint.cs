using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    private Transform _player;
    private Camera _cam;
    private GameManager _gm;
    private CameraIsObscured _cameraIsObscured;
    
    /// Please dont rate "AimingExperimental()" system,
    /// it is still under development
    
    private float _aimOffsetX;
    private float _aimOffsetY;
    private float _currentOffsetX = 0;
    private float _currentOffsetY = 0;
    private float raycastDist;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _cam = Camera.main;
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _cameraIsObscured = _cam.gameObject.GetComponent<CameraIsObscured>();
        
        _aimOffsetX = _gm.cameraOffsetModifier;
        _aimOffsetY = _gm.cameraOffsetModifier;
        raycastDist = _gm.cameraOffsetModifier;
    }
    
    void Update()
    {
        // TODO AIMING SHOULD BE MOVED TO ANOTHER PROPER SCRIPT
        if (_gm.useAimOffsetSystem)
        {
            AimingExperimental();
        }
        else
        {
            Aiming();
        }
    }

    private void FixedUpdate()
    {
        if (_gm.useAimOffsetSystem)
        {
            AimWallRaycast();
        }
    }

    void Aiming()
    {
        transform.position = _player.position + Vector3.up;
        
        if (Input.GetButton("Fire2"))
        {
            _gm.isAimingCameraMode = true;
        }
        else
        {
            _gm.isAimingCameraMode = false;
        }
    }

    void AimingExperimental()
    {
        float normalOffset = 0;

        Vector3 leftOfCam = _cam.transform.right;
        Vector3 aboveCam = _cam.transform.up;
        transform.position = _player.position + (leftOfCam * _currentOffsetX) + (aboveCam * _currentOffsetY);

        if (Input.GetButton("Fire2"))
        {
            // MUST BE REWORKED, TEMPORARY SOLUTION TO REDUCE STUTTERING, STILL STUTTERS A LITTLE
            /*if (_cameraFollow.hitsWall)
            {
                aimOffsetX = normalOffset;
            }
            else
            {
                aimOffsetX = _gm.cameraOffsetModifier;
            }*/
            
            _gm.isAimingCameraMode = true;
            if (!Mathf.Approximately(_currentOffsetX, _aimOffsetX))
            {
                _currentOffsetX = Mathf.Lerp(_currentOffsetX, _aimOffsetX, _gm.cameraToAimingLerpValue);
                //Debug.Log("Is lerping");
            }
            if (!Mathf.Approximately(_currentOffsetY, _aimOffsetY)) 
            {
                _currentOffsetY = Mathf.Lerp(_currentOffsetY, _aimOffsetY, _gm.cameraToAimingLerpValue);
            }
        }
        else
        {
            _gm.isAimingCameraMode = false;
            _currentOffsetX = Mathf.Lerp( _currentOffsetX, normalOffset, _gm.cameraToAimingLerpValue);
            _currentOffsetY = Mathf.Lerp( _currentOffsetY, normalOffset, _gm.cameraToAimingLerpValue);
        }
        
        Debug.DrawRay(_cam.transform.position, _cam.transform.right * raycastDist, Color.yellow);
        
    }

    void AimWallRaycast()
    {
        // THIS RAYCAST DOES NOT WORK AT THIS MOMENT
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.right, out hit, raycastDist, _cameraIsObscured.wallLayerMask))
        {
            _aimOffsetX = _gm.cameraOffsetModifier - (_gm.cameraOffsetModifier - hit.distance);
            //Debug.Log(("hit wall"));
        }
        else { _aimOffsetX = _gm.cameraOffsetModifier; }
    }
}
