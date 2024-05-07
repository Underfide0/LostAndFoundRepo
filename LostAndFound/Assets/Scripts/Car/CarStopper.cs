using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarStopper : MonoBehaviour
{
   
    [Header("---Wheel Colliders---")]
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;
    

    [Header("---EXTRAS---")]

    private int slowBrake = 600;
   

    private void Update()
    {
        naturalBrake();
    }
    private void naturalBrake()
    {
        _colliderBL.brakeTorque += slowBrake;
        _colliderBR.brakeTorque = slowBrake;
    }
}
