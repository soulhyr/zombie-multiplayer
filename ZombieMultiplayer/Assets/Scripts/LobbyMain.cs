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
    
    void Awake()
    {
        if (loadingUI == null)
            loadingUI = Instantiate(uiLoadingPrefab);
        
        loadingUI.Show();
        
        Init();
        AddEvents();
        
        loadingUI.Hide();
    }
    
    void OnDestroy() => ResetEvent();

    private void Init() => Pun2Manager.Instance.Init();
    
    private void ResetEvent()
    {
        btnSubmit.onClick.RemoveAllListeners();
        btnCreateRoom.onClick.RemoveAllListeners();
        btnLeaveRoom.onClick.RemoveAllListeners();

        EventDispatcher.instance.RemoveAllEventHandlers();
    }
    
    private void AddEvents()
    {
        ResetEvent();
        
        btnSubmit.onClick.AddListener(() => nicknameSubmitted(nickname.text));
        btnCreateRoom.onClick.AddListener(() => Pun2Manager.Instance.CreateRoom());
        btnLeaveRoom.onClick.AddListener(() => Pun2Manager.Instance.LeaveRoom());

        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnConnectedToMaster, type => MasterConnedted());
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnCreatedRoom, type => Debug.Log("방 생성 성공, Room 이동"));
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnJoinedRoom, type => Pun2Manager.Instance.LoadScene("Room"));
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnJoinedLobby, type => JoinedLobby());
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>(EventDispatcher.EventType.OnRoomListUpdate, (type, data) =>
        {
            Debug.Log("OnRoomListUpdate");
            RoomListUpdate(data);
        });
    }

    private void MasterConnedted()
    {
        loadingUI.Show();
        // Debug.Log("마스터 접속 성공");
        if (DataManager.Instance.nickname.Length == 0)
        {
            Debug.Log("nickname 없음");
            nicknameArea.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log($"nickname 존재 : {DataManager.Instance.nickname}");
            Pun2Manager.Instance.JoinLobby();
        }
        loadingUI.Hide();
    }

    private void JoinedLobby()
    {
        loadingUI.Show();
        Debug.Log($"[{DataManager.Instance.nickname}] 님이 로비에 왔습니다.");
        nicknameArea.SetActive(false);
        btnCreateRoom.gameObject.SetActive(true);
        uiRoomScrollview.Show();
        Debug.Log("test");
        loadingUI.Hide();
    }

    private void RoomListUpdate(List<RoomInfo> data)
    { 
        loadingUI.Show();
        uiRoomScrollview.Show();
        uiRoomScrollview.UpdateUI(data);
            
        btnLeaveRoom.gameObject.SetActive(false);
        btnCreateRoom.gameObject.SetActive(true);
        loadingUI.Hide();
    }
    
    private void nicknameSubmitted(string nick)
    {
        if (nick.Trim().Length > 0)
        {
            DataManager.Instance.nickname = nick;
            Debug.Log($"{DataManager.Instance.nickname}님이 접속하였습니다.");
            Pun2Manager.Instance.JoinLobby();
        }
        else
        {
            Debug.Log("아이디를 정확히 입력해라...");
        }
    }

    private void UpdateUI()
    {
        
    }
}