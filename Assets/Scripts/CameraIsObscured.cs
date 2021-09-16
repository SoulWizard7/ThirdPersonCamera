using UnityEngine;
using System;

public class CameraIsObscured : MonoBehaviour
{
    //Transform Variables
    private Transform _player;
    private Transform _playerCameraParent;
    private Transform _playerCamPoint;
    private Transform _regularCamPoint;
    private Vector3 _directionToCamera;
    
    private float _distance;
    private float _distanceToZoomPoint;

    //Raycast variables
    [NonSerialized]public RaycastHit[] hits;
    public LayerMask wallLayerMask;
    private RaycastHit wallHit;
    private float sphereCastRadius = 0.25f;

    private CameraFollow _cameraFollow;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _playerCameraParent = GameObject.Find("CameraPoint").transform;
        _playerCamPoint = _playerCameraParent.transform.GetChild(0);
        _regularCamPoint = _playerCameraParent.transform.GetChild(1);
        _regularCamPoint.transform.position = _playerCamPoint.transform.position;
        _cameraFollow = GetComponent<CameraFollow>();
    }
    
    private void Update()
    {
        DistanceCalculations();
        DirectionToCamera();
        
        //Debug.DrawRay(_player.transform.position, _directionToCamera * _distance, Color.yellow);
    }
    private void FixedUpdate()
    {
        CheckForWalls();
        CheckForObjectsThatShouldBeFadeOut();
    }

    void DistanceCalculations()
    {
        _distance = Vector3.Distance(transform.position, _player.position);
        _distanceToZoomPoint = Vector3.Distance(_regularCamPoint.position, _player.position) - _cameraFollow._zoom;
    }

    void DirectionToCamera()
    {
        _directionToCamera = _regularCamPoint.position - _player.position;
    }

    void CheckForWalls()
    {
        if(Physics.SphereCast(_player.position, sphereCastRadius,_directionToCamera, out wallHit, _distanceToZoomPoint, wallLayerMask))
        {
            _cameraFollow.hitsWall = true;
        }
        else 
        {
            _cameraFollow.hitsWall = false;
        }
    }
    void CheckForObjectsThatShouldBeFadeOut()
    {
        hits = Physics.BoxCastAll(_player.position, Vector3.forward,
            _directionToCamera, Quaternion.identity, _distance + 0.5f);
        
        if (hits.Length == 0) { return; }
        
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            
            //Debug.Log(hit.collider.gameObject.name);
            
            ObjectCanFade canFade = hit.transform.GetComponent<ObjectCanFade>();

            if (canFade) { canFade.enabled = true; }
        }
    }

    public float WallHit()
    {
        return wallHit.distance;
    }
}
