using System;

public class GameModel
{
    public int Player1Score { get; private set; }
    public int Player2Score { get; private set; }

    public event Action<int, int> OnScoreChanged;

    public void AddScore(int playerIndex)
    {
        if (playerIndex == 1) Player1Score++;
        else Player2Score++;

        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }

    public void ResetScores()
    {
        Player1Score = 0;
        Player2Score = 0;
        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }
}