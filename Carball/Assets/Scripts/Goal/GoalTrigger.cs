using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public int playerIndex;

    public void SendGoalEvent(int playerIndex)
    {
        object[] content = new object[] { playerIndex };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(PhotonEventCodes.GoalScored, content, options, sendOptions);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log($" Goal Scored on Player {playerIndex}'s goal! Point to Player {(playerIndex == 1 ? 2 : 1)}");
            int scoringPlayer = playerIndex == 1 ? 2 : 1; // or 2, based on your logic
            SendGoalEvent(scoringPlayer);
            //ServiceLocator.GameManager.controller.GoalScored(playerIndex);
        }
    }
}