using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.0";
    public GameObject carPlayerPrefabA;
    public GameObject carPlayerPrefabB;

    public Transform spawnPointA;
    public Transform spawnPointB;

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(); // wait to join lobby
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room found. Creating a new one.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        SpawnCar();
    }

    void SpawnCar()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.Instantiate("CarPlayerA", spawnPointA.position, spawnPointA.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate("CarPlayerB", spawnPointB.position, spawnPointB.rotation);
        }
    }
}

