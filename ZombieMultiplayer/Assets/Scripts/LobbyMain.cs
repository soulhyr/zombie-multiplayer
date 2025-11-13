using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    public UIRoomScollview uiRoomScrollview;
    public UILoading uiLoadingPrefab;
    public GameObject nicknameArea;
    public TMP_InputField nickname;
    public Button btnSubmit;
    public Button btnCreateRoom;
    public Button btnLeaveRoom;

    private UILoading loadingUI;
    void Start()
    {
        Init();
        AddEvents();
    }

    private void Init()
    {
        loadingUI = Instantiate(uiLoadingPrefab);
        loadingUI.Show();
        
        Pun2Manager.Instance.Init();
    }
    
    private void AddEvents()
    {
        btnSubmit.onClick.AddListener(() => nicknameSubmitted(nickname.text));
        // btnCreateRoom.onClick.AddListener(() => PhotonManager.Instance

        btnCreateRoom.onClick.AddListener(() =>
        {
            Debug.Log("방 생성 요청");
            Pun2Manager.Instance.CreateRoom();
        });
        
        btnLeaveRoom.onClick.AddListener(() =>
        {
            Debug.Log("방 나가기 요청");
            Pun2Manager.Instance.LeaveRoom();
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnConnectedToMaster, type =>
        {
            Debug.Log("OnConnectedToMaster");
            loadingUI.Hide();
            if (DataManager.Instance.nickname.Length == 0)
                nicknameArea.gameObject.SetActive(true);
            
            Pun2Manager.Instance.JoinLobby();
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnJoinedRoom, type =>
        {
            Debug.Log("OnJoinedRoom");
            loadingUI.Hide();
            // 룸 목록 불러오기.
            // Pun2Manager.Instance.LoadScene("ReadyLobby");
            uiRoomScrollview.Hide();
            btnCreateRoom.gameObject.SetActive(false);
            btnLeaveRoom.gameObject.SetActive(true); 
            Debug.Log($"Room count: {Pun2Manager.Instance.GetRoomCount()}");
            // PhotonNetwork.LoadLevel("ReadyLobby");
        });

        // EventDispatcher.instance.AddEventHandler<LobbyRoomInfo>(EventDispatcher.EventType.OnCreatedRoom, (type, data) =>
        // {
        //     Debug.Log("OnCreatedRoom");
        //     // DataManager.Instance.LobbyRoomInfos.Add(data);
        // });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnJoinedLobby, type =>
        {
            Debug.Log("OnJoinedLobby");
            loadingUI.Hide();
            // 룸 목록 불러오기.
            nicknameArea.SetActive(false);
            btnCreateRoom.gameObject.SetActive(true);
            uiRoomScrollview.Show();
            
            // PhotonNetwork.LoadLevel("ReadyLobby");
        });
        
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnLeftRoom, type =>
        {
            Debug.Log("OnJoinedLobby");
            
            Debug.Log($"Room count: {Pun2Manager.Instance.GetRoomCount()}");
            
            loadingUI.Hide();
            // 룸 목록 불러오기.
            btnLeaveRoom.gameObject.SetActive(false);
            btnCreateRoom.gameObject.SetActive(true);
            uiRoomScrollview.Show();
            
            // PhotonNetwork.LoadLevel("ReadyLobby");
        });
        
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>(EventDispatcher.EventType.OnRoomListUpdate, (type, data) =>
        {
            Debug.Log("OnRoomListUpdate");
            Debug.Log($"Room count: {Pun2Manager.Instance.GetRoomCount()}");
            loadingUI.Hide();
            
            // 룸 목록 불러오기.
            uiRoomScrollview.Show();
            uiRoomScrollview.UpdateUI(data);
            
            btnLeaveRoom.gameObject.SetActive(false);
            btnCreateRoom.gameObject.SetActive(true);
        });
    }
    
    private void nicknameSubmitted(string nick)
    {
        if (nick.Trim().Length > 0)
        {
            DataManager.Instance.nickname = nick;
            Debug.Log($"{DataManager.Instance.nickname}님이 접속하였습니다.");
            
            // nicknameArea.SetActive(false);
            // btnCreateRoom.gameObject.SetActive(true);
            // uiRoomScrollview.Show();
            //
            
            // btnStart.gameObject.SetActive(true);
        }
        else
        {
            // btnStart.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        
    }
}