using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class car : MonoBehaviour
{
    // Start is called before the first frame update
    public WheelCollider[] wheelColliders;
    public Transform[] wheelTrans;
    Rigidbody rb;
    float h;
    float time;
    float v;
    float speed;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = wheelColliders[0].rpm;
        time = 1;
    }
    void Update()
    {
        time -= Time.deltaTime;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Move();
        Turn();
        Tb();
        Brake();
        Unbreake();
        speed = wheelColliders[0].rpm;
    }
    void Move()
    {
        wheelColliders[0].motorTorque = v * 2000;
        wheelColliders[1].motorTorque = v * 2000;
        wheelColliders[2].motorTorque = v * 2000;
        wheelColliders[3].motorTorque = v * 2000;

    }
    void Turn()
    {
        wheelColliders[0].steerAngle = h * 50;
        wheelColliders[1].steerAngle = h * 50;
    }
    void Tb()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Vector3 pos;
            Quaternion rotation;
            wheelColliders[i].GetWorldPose(out pos, out rotation);
            wheelTrans[i].position = pos;
            wheelTrans[i].rotation = rotation;
        }
    }
        void Brake()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (speed != 0)
                {
                    wheelColliders[0].brakeTorque = 50000000;
                    wheelColliders[1].brakeTorque = 50000000;
                    wheelColliders[2].brakeTorque = 50000000;
                    wheelColliders[3].brakeTorque = 50000000;
                    time = 1.5f;

                }
            }
        }
        void Unbreake()
        {
            Debug.Log(speed);
            if (speed == 0 && time <= 0)
            {
                for(int i = 0; i < wheelColliders.Length; i++)
                {
                wheelColliders[i].brakeTorque = 0;
                }
            }
        }
        
    }


