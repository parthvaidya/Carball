using UnityEngine;
using Photon.Pun;

public class MainCarController : MonoBehaviourPun
{
    public enum PlayerControlType { Player1, Player2 }
    public PlayerControlType controlType = PlayerControlType.Player1;

    public float moveSpeed = 10f;
    public float turnSpeed = 100f;
    public float nitroBoost = 20f;
    public float deceleration = 5f;

    private Rigidbody rb;

    float moveInput;
    float turnInput;
    bool isNitro;

    [Header("Visual Wheels")]
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public float maxWheelTurnAngle = 30f;

    [Header("Rear Visual Wheels")]
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<CameraController>().SetTarget(transform, rb);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        GetInputs();
        AnimateWheels();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        // Target velocity based on input
        Vector3 targetVelocity = transform.forward * moveInput * moveSpeed;
        Vector3 currentVelocity = rb.velocity;
        targetVelocity.y = currentVelocity.y;

        // Smooth deceleration
        rb.velocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * deceleration);

        // Steering
        if (moveInput != 0)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(moveInput);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
        }

        // Nitro Boost
        if (isNitro && moveInput > 0)
        {
            rb.AddForce(transform.forward * nitroBoost, ForceMode.Impulse);
        }
    }

    void GetInputs()
    {
        if (controlType == PlayerControlType.Player1)
        {
            moveInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
            turnInput = Input.GetAxis("Horizontal");
            isNitro = Input.GetKeyDown(KeyCode.LeftShift);
        }
        else if (controlType == PlayerControlType.Player2)
        {
            moveInput = Input.GetKey(KeyCode.UpArrow) ? 1f : Input.GetKey(KeyCode.DownArrow) ? -1f : 0f;
            turnInput = (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f);
            isNitro = Input.GetKeyDown(KeyCode.G);
        }
    }



    void AnimateWheels()
    {
        float steerAngle = turnInput * maxWheelTurnAngle;

        // Steering for front wheels
        if (frontLeftWheel != null)
            frontLeftWheel.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
        if (frontRightWheel != null)
            frontRightWheel.localRotation = Quaternion.Euler(0f, steerAngle, 0f);

        // Rotation (spinning) for all wheels
        float wheelSpeed = rb.velocity.magnitude * (moveInput >= 0 ? 1 : -1);
        float rotationAmount = wheelSpeed * Time.deltaTime * 360f;

        if (frontLeftWheel != null)
            frontLeftWheel.Rotate(Vector3.right, rotationAmount);
        if (frontRightWheel != null)
            frontRightWheel.Rotate(Vector3.right, rotationAmount);
        if (rearLeftWheel != null)
            rearLeftWheel.Rotate(Vector3.right, rotationAmount);
        if (rearRightWheel != null)
            rearRightWheel.Rotate(Vector3.right, rotationAmount);
    }
}