using System.Collections.Generic;
using ExitGames.Client.Photon;
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
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public Player[] PlayerList => PhotonNetwork.PlayerList;
    public bool IsMasterClient => PhotonNetwork.IsMasterClient;
    public string NickName { get => PhotonNetwork.NickName; set => PhotonNetwork.NickName = value; }
    public bool AutomaticallySyncScene { get => PhotonNetwork.AutomaticallySyncScene; set => PhotonNetwork.AutomaticallySyncScene = value; }

    public Room CurrentRoom => PhotonNetwork.CurrentRoom;
    public bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;

    public void CreateRoom() => PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    public void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public void LoadScene(string sceneName) => PhotonNetwork.LoadLevel(sceneName);
    public void SetMyProperties(Hashtable props) => PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    public int GetRoomCount() => PhotonNetwork.CountOfRooms;
    public void LoadSceneForAll(string sceneName)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC(nameof(RPC_LoadScene), RpcTarget.AllBuffered, sceneName);
    }

    [PunRPC]
    private void RPC_LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    
    public override void OnConnectedToMaster() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnConnectedToMaster);
    public override void OnDisconnected(DisconnectCause cause) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnDisconnected);
    public override void OnJoinedLobby()
    {
        if (PhotonNetwork.InLobby)
        {
            var lobby = PhotonNetwork.CurrentLobby;
            Debug.Log($"현재 로비 이름: {lobby.Name ?? "(Default Lobby)"}");
            Debug.Log($"로비 타입: {lobby.Type}");
        }
        else
        {
            Debug.Log("아직 어떤 로비에도 속해 있지 않습니다.");
        }
        
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedLobby);
    }
    public override void OnJoinedRoom() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedRoom);
    public override void OnCreatedRoom() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnCreatedRoom);
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log($"OnCreateRoomFailed: {message}");
    public override void OnRoomListUpdate(List<RoomInfo> roomList) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnRoomListUpdate, roomList);
    public override void OnLeftRoom() => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnLeftRoom);
    public override void OnPlayerEnteredRoom(Player newPlayer) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnPlayerEnteredRoom, newPlayer);
    public override void OnPlayerLeftRoom(Player otherPlayer) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnPlayerLeftRoom, otherPlayer);
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnPlayerPropertiesUpdate, (targetPlayer, changedProps));
    public override void OnMasterClientSwitched(Player newMasterClient) => EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnMasterClientSwitched, newMasterClient);
}