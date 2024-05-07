using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("---Wheel Transforms---")]
    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformBL;
    [SerializeField] private Transform _transformBR;
    [Header("---Wheel Colliders---")]
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;
    [Header("---Steering Wheel---")]
    [SerializeField] private GameObject SteeringWheel;

    [Header("---CarLights---")]
    private bool headlights;
    [SerializeField] private GameObject HeadlightsGO;

    [Header("---EXTRAS---")]

    private bool braking;

    public float force;

    public float maxAngel;

    public int maxSpeed;

    public int maxBackSpeed;

    public int brake;

    public int slowBrake;

    private Rigidbody carRB;

    public GameObject player;

    private PlayerInput playerInput;

    private PlayerController playerController;

    
    private Vector2 Inputs()
    {
        return playerInput.actions["Movement"].ReadValue<Vector2>();
        
    }

    private void Start()
    {
        playerInput.actions["Lights"].started += CarController_Lights;

        playerInput.actions["CarBrake"].started += CarController_Braking;

        playerInput.actions["CarBrake"].canceled += CarController_canceled;

        braking = false;    
    }

    private void CarController_canceled(InputAction.CallbackContext obj)
    {
        braking = false;
    }

    private void CarController_Lights(InputAction.CallbackContext obj)
    {
        
        LightsOn();

        
    }
    private void CarController_Braking(InputAction.CallbackContext obj)
    {
        forceBrake();
    }

    private void Awake()
    {
        carRB = GetComponent<Rigidbody>();

        playerInput = player.GetComponent<PlayerInput>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        _colliderBL.motorTorque = force * Inputs().y;
        _colliderBR.motorTorque = force * Inputs().y;

        _colliderFL.steerAngle = maxAngel * Inputs().x;
        _colliderFR.steerAngle = maxAngel * Inputs().x;


        RotateWheel(_colliderBL, _transformBL);
        RotateWheel(_colliderBR, _transformBR);
        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFR, _transformFR);


        if (!braking)
        {
            _colliderBL.brakeTorque = 0;
            _colliderBR.brakeTorque = 0;
        }
        
         
        

        SteeringWheel.transform.localRotation = Quaternion.Euler(0,0, Input.GetAxis("Horizontal") * 35);

        
        if (Inputs().y == 0)
        {
            naturalBrake();
        }

        
        if (carRB.velocity.magnitude > maxSpeed)
        {
            carRB.velocity = Vector3.ClampMagnitude(carRB.velocity, maxSpeed);
        }

       
    }

    private void RotateWheel(WheelCollider col, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }

   private void LightsOn()
    {
        if (headlights && playerController.isDriving)
        {
            HeadlightsGO.SetActive(false);
            headlights = false;
        }
        else if (playerController.isDriving)
        {
            HeadlightsGO.SetActive(true);
            headlights = true;
        }
    }

    private void forceBrake()
    {
        braking = true;
        _colliderBL.brakeTorque = brake;
        _colliderBR.brakeTorque = brake;
    }

    private void naturalBrake()
    {
        _colliderBL.brakeTorque += slowBrake;
        _colliderBR.brakeTorque = slowBrake;   
    }
}
