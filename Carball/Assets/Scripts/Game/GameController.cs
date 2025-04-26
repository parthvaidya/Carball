using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameModel model;

    public void Init(GameModel gameModel)
    {
        model = gameModel;
    }

    public void GoalScored(int playerIndex)
    {
        model.AddScore(playerIndex);
        ServiceLocator.GameManager.RestartRound(); // Reset ball and players
    }
}