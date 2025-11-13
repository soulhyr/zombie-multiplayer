using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Pun2Manager : MonoBehaviourPunCallbacks
{
    private static Pun2Manager instance;
    public static Pun2Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<Pun2Manager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Pun2Manager");
                    instance = go.AddComponent<Pun2Manager>();
                }
            }
            return instance;
        }
    }

    private string gameVersion = "1";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    public void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public void LoadScene(string sceneName) => PhotonNetwork.LoadLevel(sceneName);

    public int GetRoomCount() => PhotonNetwork.CountOfRooms;

    public bool IsMasterClient => PhotonNetwork.IsMasterClient;
    public string NickName => PhotonNetwork.NickName;

    public override void OnConnectedToMaster() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnConnectedToMaster);
    public override void OnJoinedLobby() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedLobby);
    public override void OnDisconnected(DisconnectCause cause) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnDisconnected);
    public override void OnJoinedRoom() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedRoom);
    public override void OnCreatedRoom() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnCreatedRoom);
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log($"OnCreateRoomFailed: {message}");
    public override void OnRoomListUpdate(List<RoomInfo> roomList) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnRoomListUpdate, roomList);
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnLeftRoom);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) => Debug.Log("OnPlayerLeftRoom");
}