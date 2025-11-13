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
    
    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler(EventDispatcher.EventType.OnConnectedToMaster);
        EventDispatcher.instance.RemoveEventHandler(EventDispatcher.EventType.OnCreatedRoom);
        EventDispatcher.instance.RemoveEventHandler(EventDispatcher.EventType.OnJoinedLobby);
        EventDispatcher.instance.RemoveEventHandler(EventDispatcher.EventType.OnRoomListUpdate);
    }

    private void Init()
    {
        if (loadingUI == null)
        {
            loadingUI = Instantiate(uiLoadingPrefab);
            DontDestroyOnLoad(loadingUI.gameObject); // 씬 전환에도 유지
        }

        loadingUI.Show();
        Pun2Manager.Instance.Init();
    }
    
    private void AddEvents()
    {
        btnSubmit.onClick.AddListener(() => nicknameSubmitted(nickname.text));
        btnCreateRoom.onClick.AddListener(() => Pun2Manager.Instance.CreateRoom());
        btnLeaveRoom.onClick.AddListener(() => Pun2Manager.Instance.LeaveRoom());
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnConnectedToMaster, type =>
        {
            // Debug.Log("마스터 접속 성공");
            loadingUI.Hide();
            if (DataManager.Instance.nickname.Length == 0)
            {
                // Debug.Log("nickname 없음");
                nicknameArea.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log($"nickname 존재 : {DataManager.Instance.nickname}");
                Pun2Manager.Instance.JoinLobby();
            }
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnCreatedRoom, type =>
        {
            Debug.Log("방 생성 성공, Room 이동");
            Pun2Manager.Instance.LoadScene("Room");
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnJoinedLobby, type =>
        {
            // Debug.Log("로비 접속 성공");
            Debug.Log($"[{DataManager.Instance.nickname}] 님이 로비에 왔습니다.");
            nicknameArea.SetActive(false);
            btnCreateRoom.gameObject.SetActive(true);
            uiRoomScrollview.Show();
            loadingUI.Hide();
        });
        
        
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>(EventDispatcher.EventType.OnRoomListUpdate, (type, data) =>
        {
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