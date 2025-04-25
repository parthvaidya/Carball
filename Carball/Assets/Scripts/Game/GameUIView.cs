
using TMPro;
using UnityEngine;

public class GameUIView : MonoBehaviour, IGameEvents
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public void OnScoreChanged(int p1, int p2)
    {
        scoreText.text = $"P1:{p1} - P2:{p2}";
    }

    public void OnTimerTick(int secondsLeft)
    {
        timerText.text = $"Time: {secondsLeft}s";
    }

    public void OnMatchEnded()
    {
        timerText.text = "Match Over!";
    }
}