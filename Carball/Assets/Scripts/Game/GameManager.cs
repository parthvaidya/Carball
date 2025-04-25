using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun
{
    public GameUIView view;
    public GameController controller;

    private GameModel model;

    private int matchDuration = 60;
    private float timer;
    private bool matchEnded = false;

    void Awake()
    {
        ServiceLocator.Register(this);
        model = new GameModel();
        controller.Init(model);
        model.OnScoreChanged += view.OnScoreChanged;
    }

    void Start()
    {
        StartCoroutine(MatchTimer());
    }

    public void RestartRound()
    {
        // Reset ball and player positions
        // You’ll fill this based on your scene setup
    }

    IEnumerator MatchTimer()
    {
        timer = matchDuration;

        while (timer > 0)
        {
            view.OnTimerTick((int)timer);
            yield return new WaitForSeconds(1f);
            timer--;
        }

        view.OnMatchEnded();
        matchEnded = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1); // Go to next scene
    }
}
