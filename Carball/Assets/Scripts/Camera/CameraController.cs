using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Rigidbody playerRB;
    public Vector3 Offset;
    public float speed;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update

    public void SetTarget(Transform newPlayer, Rigidbody newRB)
    {
        player = newPlayer;
        playerRB = newRB;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    //void LateUpdate()
    //{

    //    if (player == null || playerRB == null)
    //        return;


    //    Vector3 playerForward = (playerRB.velocity + player.transform.forward).normalized;
    //    transform.position = Vector3.Lerp(transform.position,
    //        player.position + player.transform.TransformVector(Offset)
    //        + playerForward * (-5f),
    //        speed * Time.deltaTime);
    //    transform.LookAt(player);
    //}

    void LateUpdate()
    {
        if (player == null || playerRB == null)
            return;

        Vector3 playerForward = (playerRB.velocity.magnitude > 0.1f)
            ? (playerRB.velocity + player.transform.forward).normalized
            : player.transform.forward;

        Vector3 targetPosition = player.position
            + player.transform.TransformVector(Offset)
            + playerForward * -5f;

        transform.position = targetPosition;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.15f);
        transform.LookAt(player);
    }
}
