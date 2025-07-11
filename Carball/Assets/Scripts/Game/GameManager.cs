using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameUIView view;
    public GameController controller;

    private GameModel model;

    private int matchDuration = 120;
    private float timer;
    private bool matchEnded = false;

    void Awake()
    {
        ServiceLocator.Register(this);
        model = new GameModel();
        controller.Init(model);
        model.OnScoreChanged += view.OnScoreChanged;
    }

    //void Start()
    //{
    //    LoadScoreFromRoom();
    //    StartCoroutine(MatchTimer());
    //    view.OnScoreChanged(model.Player1Score, model.Player2Score);
    //}

    void Start()
    {
        LoadScoreFromRoom();
        view.OnScoreChanged(model.Player1Score, model.Player2Score);
        TryStartMatch(); // only starts when both players are present
    }

    public void RestartRound()
    {
        // Reset ball and player positions
        // You’ll fill this based on your scene setup
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void TryStartMatch()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartMatch", RpcTarget.All);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);
        TryStartMatch(); // check again now that someone joined
    }

    [PunRPC]
    void StartMatch()
    {
        if (!matchEnded) // prevent duplicate starts
        {
            StartCoroutine(MatchTimer());
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == PhotonEventCodes.GoalScored)
        {
            object[] data = (object[])photonEvent.CustomData;
            int playerIndex = (int)data[0];

            Debug.Log($"Received Goal Event for Player {playerIndex}");
            controller.GoalScored(playerIndex);
        }
    }


    void LoadScoreFromRoom()
    {
        if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom.CustomProperties == null)
            return;

        var props = PhotonNetwork.CurrentRoom.CustomProperties;

        int p1 = props.ContainsKey(RoomPropertyKeys.Player1Score) ? (int)props[RoomPropertyKeys.Player1Score] : 0;
        int p2 = props.ContainsKey(RoomPropertyKeys.Player2Score) ? (int)props[RoomPropertyKeys.Player2Score] : 0;

        // Set the values directly into model
        typeof(GameModel).GetProperty("Player1Score").SetValue(model, p1);
        typeof(GameModel).GetProperty("Player2Score").SetValue(model, p2);
        model.SetScores(p1, p2);
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
