using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarController : MonoBehaviourPun
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        //public GameObject wheelEffectObj;
        //public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;
    public float nitroForce = 15000f;  // Multiplies acceleration when using nitro
    private bool isNitro = false;
    private bool isBraking = false;

    public Rigidbody carRigidbody;


    void Start()
    {

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<CameraController>().SetTarget(transform, carRigidbody);
        }
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        GetInputs();
        AnimateWheels();
        //WheelEffects();
    }

    void LateUpdate()
    {
        if (!photonView.IsMine) return;
        Move();
        Steer();
        Brake();
        NitroBoost();
    }

    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            isNitro = Input.GetKeyDown(KeyCode.LeftShift);
            isBraking = Input.GetKey(KeyCode.Space);

            if (Input.GetKey(KeyCode.W))
            {
                moveInput = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveInput = -1f;
            }
            else
            {
                moveInput = 0f;
            }

            steerInput = Input.GetAxis("Horizontal");
        }
    }


    void Move()
    {
        foreach (var wheel in wheels)
        {
            //wheel.wheelCollider.motorTorque = moveInput * 6000 * maxAcceleration * Time.deltaTime;
            wheel.wheelCollider.motorTorque = moveInput * 600f * maxAcceleration; //can use tick , use velocity instad of torque
        }
    }

    void NitroBoost()
    {
        if (isNitro && moveInput > 0)
        {
            carRb.AddForce(transform.forward * nitroForce, ForceMode.Impulse);
            isNitro = false; // Reset to avoid repeated triggers
        }
    }
    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (isBraking || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 30000f * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    //void WheelEffects()
    //{
    //    foreach (var wheel in wheels)
    //    {
    //        //var dirtParticleMainSettings = wheel.smokeParticle.main;

    //        if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f)
    //        {
    //            wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
    //            wheel.smokeParticle.Emit(1);
    //        }
    //        else
    //        {
    //            wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
    //        }
    //    }
    //}
}
