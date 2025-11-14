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
    public GameObject woman;
    public TMP_Text txtNickname;
    
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

        EventDispatcher.instance.RemoveAllEventHandlers();
    }
    
    private void AddEvents()
    {
        ResetEvent();
        
        btnSubmit.onClick.AddListener(() => NicknameSubmitted(nickname.text));
        btnCreateRoom.onClick.AddListener(() => Pun2Manager.Instance.CreateRoom());

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
        if (Pun2Manager.Instance.NickName.Length == 0)
        {
            Debug.Log("nickname 없음");
            nicknameArea.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log($"nickname 존재 : {Pun2Manager.Instance.NickName}");
            Pun2Manager.Instance.JoinLobby();
        }
        loadingUI.Hide();
    }

    private void JoinedLobby()
    {
        loadingUI.Show();
        Debug.Log($"[{Pun2Manager.Instance.NickName}] 님이 로비에 왔습니다.");
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
            
        btnCreateRoom.gameObject.SetActive(true);
        loadingUI.Hide();
    }
    
    private void NicknameSubmitted(string nick)
    {
        loadingUI.Show();
        if (nick.Trim().Length > 0)
        {
            Pun2Manager.Instance.NickName = nick;
            Debug.Log($"{Pun2Manager.Instance.NickName}님이 접속하였습니다.");
            woman.SetActive(true);
            txtNickname.gameObject.SetActive(true);
            txtNickname.text = Pun2Manager.Instance.NickName;
            Pun2Manager.Instance.JoinLobby();
        }
        else
        {
            Debug.Log("아이디를 정확히 입력해라...");
        }
        loadingUI.Hide();
    }
}