using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pun2Manager : MonoBehaviourPunCallbacks
{
    private static readonly Pun2Manager instance = new Pun2Manager();
    public static Pun2Manager Instance => instance;
    private string gameVersion = "1";

    private Pun2Manager() { }
    public void Init()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    // private void Connect()
    // {
    //     Debug.Log($"IsConnected : {PhotonNetwork.IsConnected}");
    //     if (PhotonNetwork.IsConnected)
    //     {
    //         PhotonNetwork.JoinRandomRoom();
    //     }
    //     else
    //     {
    //         PhotonNetwork.ConnectUsingSettings();
    //     }
    // }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        // EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnConnectedToMaster);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        // EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnConnectedToMaster);
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public int GetRoomCount()
    {
        return PhotonNetwork.CountOfRooms;
    }

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
        Debug.Log($"isMaster: {PhotonNetwork.IsMasterClient}");
        // Debug.Log(GetRoomCount());

        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnJoinedRoom);
    }

    // public override void OnJoinRandomFailed(short returnCode, string message)
    // {
    //     Debug.Log($"OnJoinRandomFailed, returnCode: {returnCode}, message: {message}");
    //     
    //     PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    // }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom, 서버에 방 데이터 업데이트");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed, returnCode: {returnCode}, message: {message}, 실패 처리 필요");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnRoomListUpdate, roomList);
    }

    public override void OnLeftRoom()
    {
        Debug.Log($"PhotonNetwork.InLobby: {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
        EventDispatcher.instance.SendEvent(EventDispatcher.EventType.OnLeftRoom);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
    }
}