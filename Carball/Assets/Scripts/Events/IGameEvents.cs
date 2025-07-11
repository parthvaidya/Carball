public interface IGameEvents
{
    void OnScoreChanged(int player1Score, int player2Score);
    void OnTimerTick(int secondsRemaining);
    void OnMatchEnded();
}