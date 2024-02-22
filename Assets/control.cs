using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController1 : MonoBehaviour
{
    public float horizontalInput, verticalInput;
    public float currentSteerAngle, currentbreakForce;
    public bool isBreaking;
    public float motor = 0f;
    public  bool isstart = false;
    public bool is_ai = false;
    public bool VariableValue { get; private set; }
    public bool VariableValue_ai { get; private set; }
    public float verticalvalue { get; private set; }
    // Settings
    [SerializeField] public float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] public Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] public Transform rearLeftWheelTransform, rearRightWheelTransform;

    public void FixedUpdate()
    {
        GetInput();

        if (isstart==true)
        {
            if (is_ai == false)
            {
                HandleMotor();
            }
            else
            {
                ai_handlemotor();
            }
            HandleSteering();
            UpdateWheels();
        }

    }
    public void ReceiveVariableValue(bool value)
    {
 
        VariableValue = value;
        isstart = VariableValue;
    }
    public void Receivevertical(float value)
    {
        verticalvalue = value;
        motor = verticalvalue;
    }
    public void Recive_ai_signal(bool value)
    {
        VariableValue_ai = value;
        is_ai = VariableValue_ai;
    }
    public void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
        if(Input.GetKey(KeyCode.F))
        {
            if (isstart == false)
            {
                isstart = true;
            }
            else
            {
                isstart = false;
            }
        }

    }
    public void modifyvalue(bool  value)
    {
        isstart = value;
    }

   public void ai_handlemotor()
    {
        frontLeftWheelCollider.motorTorque = motor;
        frontRightWheelCollider.motorTorque = motor;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }
    public void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    public void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    public void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    public void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    public void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}

