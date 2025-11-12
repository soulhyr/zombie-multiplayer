using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Button btn;
    public TMP_InputField txtNickname;
    public GameObject txtInfo;
    
    private string gameVersion = "1";
    
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        
        btn.interactable = false;
        btn.onClick.AddListener(() =>
        {
            if (txtNickname.text != "")
            {
                txtInfo.SetActive(false);
                DataManager.Instance.SetName(txtNickname.text);
                PhotonNetwork.NickName = DataManager.Instance.nickname;
                Debug.Log("룸 접속 요청");
                btn.interactable = false;
                Connect();
            }
            else
            {
                txtInfo.SetActive(true);
            }
        });        
    }

    private void Connect()
    {
        Debug.Log($"IsConnected : {PhotonNetwork.IsConnected}");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        btn.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
        btn.interactable = false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log($"isMaster: {PhotonNetwork.IsMasterClient}");

        var me = PhotonNetwork.LocalPlayer;
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p == me) continue;
            Debug.Log($"[{p.NickName}]님이 입장 했습니다.");
        }
        Debug.Log($"[{PhotonNetwork.NickName}]님이 입장 했습니다.");
        
        PhotonNetwork.LoadLevel("Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRandomFailed, returnCode: {returnCode}, message: {message}");
        
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed, returnCode: {returnCode}, message: {message}");
    }

    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[{newPlayer.NickName}]님이 입장 했습니다.");
    }
}