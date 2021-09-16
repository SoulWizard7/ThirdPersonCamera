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
    private CameraFollow _cameraFollow;
    
    private float aimOffset;
    
    private float currentOffset = 0;
    
    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _cam = Camera.main;
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        aimOffset = _gm.cameraOffsetModifier;
        _cameraIsObscured = _cam.gameObject.GetComponent<CameraIsObscured>();
        _cameraFollow = _cam.gameObject.GetComponent<CameraFollow>();
    }
    
    void Update()
    {
        // TODO AIMING SHOULD BE MOVED TO ANOTHER PROPER SCRIPT
        Aiming();
    }
    
    void Aiming()
    {
        float normalOffset = 0;

        Vector3 leftOfCam = _cam.transform.right;
        Vector3 aboveCam = _cam.transform.up;
        transform.position = _player.position + (leftOfCam * currentOffset) + (aboveCam * (currentOffset / 2));

        if (Input.GetButton("Fire2"))
        {
            // MUST BE REWORKED, TEMPORARY SOLUTION TO REDUCE STUTTERING, STILL STUTTERS A LITTLE
            if (_cameraFollow.hitsWall)
            {
                aimOffset = normalOffset;
            }
            else
            {
                aimOffset = _gm.cameraOffsetModifier;
            }
            
            _gm.isAimingCameraMode = true;
            if (!Mathf.Approximately(currentOffset, aimOffset))
            {
                currentOffset = Mathf.Lerp(currentOffset, aimOffset, _gm.cameraToAimingLerpValue);
            }
        }
        else
        {
            _gm.isAimingCameraMode = false;
            currentOffset = Mathf.Lerp( currentOffset, normalOffset, _gm.cameraToAimingLerpValue);
        }

        // THIS RAYCAST DOES NOT WORK AT THIS MOMENT
        /*RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, Vector3.right, out hit, aimOffset, _cameraIsObscured.wallLayerMask))
        {
            aimOffset = hit.distance;
            Debug.Log(("hit wall"));
        }
        else { aimOffset = _gm.cameraOffsetModifier; }*/
    }
}
