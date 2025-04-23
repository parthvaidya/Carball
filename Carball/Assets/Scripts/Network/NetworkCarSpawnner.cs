using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkCarSpawner : MonoBehaviourPunCallbacks
{
    public GameObject carPlayerPrefabA;
    public GameObject carPlayerPrefabB;

    public Transform spawnPointA;
    public Transform spawnPointB;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
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
