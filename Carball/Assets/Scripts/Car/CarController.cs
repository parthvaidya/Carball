using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public WheelColliders colliders;
    public WheelMeshes wheelmesh;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyWheelPositions();
    }
    void ApplyWheelPositions()
    {
        UpdateWheel(colliders.FrontRightWheel, wheelmesh.FrontRightWheel);
        UpdateWheel(colliders.FrontLeftWheel, wheelmesh.FrontLeftWheel);
        UpdateWheel(colliders.RearLeftWheel, wheelmesh.RearLeftWheel);
        UpdateWheel(colliders.RearRightWheel, wheelmesh.RearRightWheel);
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


}
