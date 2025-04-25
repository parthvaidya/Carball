using Photon.Pun;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public int playerIndex;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log($" Goal Scored on Player {playerIndex}'s goal! Point to Player {(playerIndex == 1 ? 2 : 1)}");

            ServiceLocator.GameManager.controller.GoalScored(playerIndex);
        }
    }
}