using Photon.Pun;
using System;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameModel
{
    public int Player1Score { get; private set; }
    public int Player2Score { get; private set; }

    public event Action<int, int> OnScoreChanged;


    public void AddScore(int playerIndex)
    {
        if (playerIndex == 1) Player1Score++;
        else Player2Score++;

        // Update Photon Room Properties
        if (PhotonNetwork.InRoom)
        {
            var props = new Hashtable
        {
            { RoomPropertyKeys.Player1Score, Player1Score },
            { RoomPropertyKeys.Player2Score, Player2Score }
        };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }

    public void SetScores(int p1, int p2)
    {
        Player1Score = p1;
        Player2Score = p2;
        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }

    public void ResetScores()
    {
        Player1Score = 0;
        Player2Score = 0;
        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }
}