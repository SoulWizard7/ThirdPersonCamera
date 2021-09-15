using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement Variables
    private float speed = 12f;
    Vector3 velocity;
    private Vector3 _move;
    
    // Gravity & Jump Variables
    private float gravity = -9.81f;
    public float gravityModifier = 1f;
    public float jumpHeight = 3f;
    
    // isGrounded Variables
    [Header("Grounded")] public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    
    // Rotate & Mouse Variables
    public float rotateSpeed = 1f;
    public float newRotateSpeed = 1f;
    private float x;
    private float z;
    private float turnSmoothVelocity = 0f;
    private float turnSmoothTime = 0.1f;

    // Other Components
    private GameManager _gm;
    private CameraFollow _cameraFollow;
    private Transform _cam;
    private CharacterController controller;

    private void Awake()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _cam = Camera.main.transform;
        _cameraFollow = _cam.GetComponent<CameraFollow>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundedAndJump();
        SprintButton();
        Gravity();
        
        if (_gm.isAimingCameraMode)
        {
            AimMovement();
        }
        else
        {
            RegularMovement();
        }
        
        //GetMove();
    }
    
    void GroundedAndJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }
    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    void Gravity()
    {
        velocity.y += (gravity * gravityModifier) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void RegularMovement()
    {
        z = Input.GetAxisRaw("Vertical");
        x = Input.GetAxisRaw("Horizontal");

        _move = _cam.right * x + _cam.forward * z;
        _move.y = 0;
        _move = _move.normalized;

        if (_move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(_move.x, _move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        
        controller.Move(_move * speed * Time.deltaTime);
    }

    void AimMovement()
    {
        z = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");
        _move = _cam.right * x + _cam.forward * z;
        _move.y = 0;
        _move = _move.normalized;

        controller.Move(_move * speed * Time.deltaTime);

        float aimAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cam.transform.rotation.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, aimAngle, transform.rotation.eulerAngles.z);
    }

    void SprintButton()
    {
        // Sprint button
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10;
        }
        else
        {
            speed = 5;
        }
    }

    // for potential enemy AI to take players movement in consideration
    public Vector3 GetMove()
    {
        return _move;
    }
}