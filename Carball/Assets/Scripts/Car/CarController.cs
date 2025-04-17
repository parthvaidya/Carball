using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelmesh;
    //public WheelParticles wheelParticles;
    public float gasInput;
    public float breakInput;
    public float steeringInput;
    
    public float motorPower;
    public float brakePower;
    public float nitroBoost = 2000f;
    //public float breakSpeed;
    private float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        ApplyWheelPositions();
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, rb.velocity - transform.forward);
        if(slipAngle < 120f)
        {
            if(gasInput < 0 && Vector3.Dot(transform.forward, rb.velocity) > 0)
            {
                breakInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            
            
        }
        else
        {
            breakInput = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb.AddForce(transform.forward * nitroBoost, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            breakInput = 1f; // Max brake
        }
    }

    void ApplyBrake()
    {
        float brakeForce = breakInput * brakePower;
        colliders.FrontRightWheel.brakeTorque = brakeForce * 0.7f;
        colliders.FrontLeftWheel.brakeTorque = brakeForce * 0.7f;
        colliders.RearLeftWheel.brakeTorque = brakeForce * 0.3f;
        colliders.RearRightWheel.brakeTorque = brakeForce * 0.3f;
    }

    void ApplyMotor()
    {
        colliders.RearRightWheel.motorTorque = motorPower * gasInput;
        colliders.RearLeftWheel.motorTorque = motorPower * gasInput;
    }
    void ApplyWheelPositions()
    {
        UpdateWheel(colliders.FrontRightWheel, wheelmesh.FrontRightWheel);
        UpdateWheel(colliders.FrontLeftWheel, wheelmesh.FrontLeftWheel);
        UpdateWheel(colliders.RearLeftWheel, wheelmesh.RearLeftWheel);
        UpdateWheel(colliders.RearRightWheel, wheelmesh.RearRightWheel);
    }

    //void ApplySteering()
    //{
    //    float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
    //    steeringAngle += Vector3.SignedAngle(transform.forward, rb.velocity, Vector3.up);
    //    steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
    //    colliders.FrontRightWheel.steerAngle = steeringAngle;
    //    colliders.FrontLeftWheel.steerAngle = steeringAngle;
    //}

    void ApplySteering()
    {
        // Detect whether the car is moving forward or in reverse
        float direction = Vector3.Dot(transform.forward, rb.velocity) >= 0 ? 1f : -1f;

        float maxSteeringAngle = 30f; // Max steering angle in degrees
        float steeringAngle = steeringInput * maxSteeringAngle * direction;

        // Clamp for safety
        steeringAngle = Mathf.Clamp(steeringAngle, -maxSteeringAngle, maxSteeringAngle);

        // Apply to front wheels
        colliders.FrontLeftWheel.steerAngle = steeringAngle;
        colliders.FrontRightWheel.steerAngle = steeringAngle;
    }

    void UpdateWheel(WheelCollider coll , MeshRenderer wheelmesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelmesh.transform.position = position;
        wheelmesh.transform.rotation = quat;
    }

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FrontLeftWheel;
        public WheelCollider FrontRightWheel;
        public WheelCollider RearLeftWheel;
        public WheelCollider RearRightWheel;

    }

    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer FrontLeftWheel;
        public MeshRenderer FrontRightWheel;
        public MeshRenderer RearLeftWheel;
        public MeshRenderer RearRightWheel;

    }


    //public class  WheelParticles 
    //{
    //    public ParticleSystem FrontLeftWheel;
    //    public ParticleSystem FrontRightWheel;
    //    public ParticleSystem RearLeftWheel;
    //    public ParticleSystem RearRightWheel;
    //}

}
