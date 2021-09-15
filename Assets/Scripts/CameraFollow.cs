using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Transforms & Vectors
    [NonSerialized]public Transform _playerCameraParent;
    private Transform _playerCamPoint;

    private Vector3 _whereCamWantsToBe;
    private Vector3 _currentVelocity = Vector3.zero;
    
    //Mouse Look Variables
    private float _mouseY;
    public float yRotateSpeed = 10;
    public float xRotateSpeed = 2000;
    private float _smoothDampSpeed = 0.05f;
    public float wallOffset;

    //Zoom Variables
    public float zoomSpeed;
    private float _tempZoom = 0f;
    [NonSerialized] public float _zoom = 0f;
    public float minMaxZoomDistance = 7;

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
        float rotateMoveX = mouseX * xRotateSpeed * Time.deltaTime;
        _playerCameraParent.transform.RotateAround(transform.position, Vector3.up, rotateMoveX);
    }

    void MouseLookHorizontal()
    {
        //Mouse Look up/down
        if(_gm.invertMouseLook) {_mouseY += Input.GetAxis("Mouse Y") * yRotateSpeed * Time.deltaTime;}
        else {_mouseY -= Input.GetAxis("Mouse Y") * yRotateSpeed * Time.deltaTime;}
        
        _mouseY = Mathf.Clamp(_mouseY, -80, 45);

        _playerCameraParent.transform.rotation = Quaternion.Euler(
            _mouseY,
            _playerCameraParent.transform.eulerAngles.y,
            _playerCameraParent.transform.eulerAngles.z);
    }

    void CamZoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            MouseWheelZoom(Input.GetAxisRaw("Mouse ScrollWheel"));
        }
    }

    void MouseWheelZoom(float direction)
    {
        _tempZoom = direction * zoomSpeed;
        _zoom += _tempZoom;

        if (_zoom > minMaxZoomDistance) _zoom = minMaxZoomDistance;
        if (_zoom < -minMaxZoomDistance) _zoom = -minMaxZoomDistance;
    }

    void CamLookAtPlayer()
    {
        transform.LookAt(_playerCameraParent.transform);
    }

    void CamFollow()
    {
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