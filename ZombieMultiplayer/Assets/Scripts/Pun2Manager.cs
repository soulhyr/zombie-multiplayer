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
                instance = FindObjectOfType<Pun2Manager>();

                // 만약 씬에 없으면 생성
                if (instance == null)
                {
                    GameObject go = new GameObject("Pun2Manager");
                    instance = go.AddComponent<Pun2Manager>();
                }

                // 씬 전환 시에도 파괴되지 않게 설정
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private string gameVersion = "1";

    private void Awake()
    {
        // 중복 방지 (기존 인스턴스가 이미 존재하면 자신 파괴)
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

    public override void OnConnectedToMaster()
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnConnectedToMaster);
    }

    public override void OnJoinedLobby()
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedLobby);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnDisconnected);
    }

    public override void OnJoinedRoom()
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedRoom);
    }

    public override void OnCreatedRoom()
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnCreatedRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed: {message}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnRoomListUpdate, roomList);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnLeftRoom);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
    }
}