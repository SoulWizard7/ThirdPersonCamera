using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Transforms & Vectors
    private Transform _playerCameraParent;
    private Transform _playerCamPoint;
    private Vector3 _whereCamWantsToBe;
    private Vector3 _currentVelocity = Vector3.zero;
    
    //Mouse Look Variables
    private float _mouseY;
    public float mouseSensitivity = 2000;
    private const float _smoothDampSpeed = 0.08f;
    
    //private float wallOffset = 0.8f;
    private const float mouseLookMax = 45f;
    private const float mouseLookMin = -80f;
    private const float aimMouseLookMin = -45f;

    //Zoom Variables
    public float zoomSpeed;
    [NonSerialized]public float _zoom = 0f;
    private const float minMaxZoomDistance = 7;
    private float _tempZoom;
    private bool switchBack;

    //Other Components
    [NonSerialized] public bool hitsWall;
    private GameManager _gm;
    private CameraIsObscured _cameraIsObscured;

    private void Awake()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerCameraParent = GameObject.Find("CameraPoint").transform;
        _playerCamPoint = _playerCameraParent.transform.GetChild(0);
        transform.position = _playerCamPoint.transform.position;
        _cameraIsObscured = GetComponent<CameraIsObscured>();
    }

    private void Update()
    {
        CamFollow();
        CamLookAtPlayer();
        CamZoom();
        MouseLookHorizontal();
        MouseLookVertical();
    }

    void MouseLookVertical()
    {
        //mouseLook sideways
        float mouseX = Input.GetAxis("Mouse X");
        float rotateMoveX = mouseX * mouseSensitivity * Time.deltaTime;
        _playerCameraParent.transform.RotateAround(transform.position, Vector3.up, rotateMoveX);
    }

    void MouseLookHorizontal()
    {
        //Mouse Look up/down
        if(_gm.invertMouseLook) {_mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;}
        else {_mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;}
        
        _mouseY = Mathf.Clamp(_mouseY, _gm.isAimingCameraMode ? aimMouseLookMin : mouseLookMin, mouseLookMax);

        _playerCameraParent.transform.rotation = Quaternion.Euler(
            _mouseY,
            _playerCameraParent.transform.eulerAngles.y,
            _playerCameraParent.transform.eulerAngles.z);
    }

    void CamZoom()
    {
        if(Input.GetButtonDown("Fire2")){ _tempZoom = _zoom; }
        if(Input.GetButtonUp("Fire2")) { switchBack = true; }
        
        if (_gm.isAimingCameraMode)
        {
            _zoom = _gm.aimZoom;
            switchBack = true;
        }
        else if (!_gm.isAimingCameraMode && switchBack)
        {
            switchBack = false;
            _zoom = _tempZoom;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            MouseWheelZoom(Input.GetAxisRaw("Mouse ScrollWheel"));
        }
    }

    void MouseWheelZoom(float direction)
    {
        float zoom = direction * zoomSpeed;
        _zoom += zoom;
        
        if (_zoom > minMaxZoomDistance) _zoom = minMaxZoomDistance;
        if (_zoom < -minMaxZoomDistance) _zoom = -minMaxZoomDistance;
    }

    void CamLookAtPlayer()
    {
        transform.LookAt(_playerCameraParent.transform);
    }

    void CamFollow()
    {
        float wallOffset = 0.8f;
        
        if (!hitsWall)
        {
            _whereCamWantsToBe = Vector3.MoveTowards(_playerCamPoint.transform.position, _playerCameraParent.position, _zoom);
        }
        else
        {
            _whereCamWantsToBe = Vector3.MoveTowards(_playerCameraParent.position, _playerCamPoint.transform.position,
                _cameraIsObscured.WallHit() - wallOffset);
        }
        // WITH LERP
        //transform.position = Vector3.Lerp(transform.position, _playerCamPoint.transform.position, lerpSpeed * Time.deltaTime);

        // WITH SMOOTHDAMP
        transform.position = Vector3.SmoothDamp(transform.position, _whereCamWantsToBe,
            ref _currentVelocity, _smoothDampSpeed);
    }
}