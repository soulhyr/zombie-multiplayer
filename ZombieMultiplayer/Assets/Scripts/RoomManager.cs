using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public GameObject woman1;
    public GameObject woman2;
    public Button btnBack;
    public Button btnStart;
    public Button btnReady;
    public TMP_Text nickname1;
    public TMP_Text nickname2;

    public UILoading uiLoadingPrefab;
    private UILoading loadingUI;
    
    private bool isReady;
    
    void Start()
    {
        loadingUI = Instantiate(uiLoadingPrefab);
        loadingUI.Show();
        
        Init();
        AddEvents();
        
        loadingUI.Hide();
    }

    private void Init()
    {
        
        SetButton(btnStart, PhotonNetwork.IsMasterClient);
        SetButton(btnStart, !PhotonNetwork.IsMasterClient);
        btnStart.interactable = false;
        btnStart.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        btnReady.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
        
        
        if (PhotonNetwork.IsMasterClient)
        {
            woman1.SetActive(true);
            Debug.Log("nic : " + DataManager.Instance.nickname);
            nickname1.text = DataManager.Instance.nickname;
        }
        else
        {
            woman2.SetActive(true);
            nickname2.text = DataManager.Instance.nickname;
        }
    }

    
    private void AddEvents()
    {
        btnBack.onClick.AddListener(() =>
        {
            Debug.Log("btnBack");
            Pun2Manager.Instance.LeaveRoom();
        });
        btnStart.onClick.AddListener(() => PhotonNetwork.LoadLevel("Main"));
        btnReady.onClick.AddListener(() =>
        {
            btnReady.GetComponentInChildren<TMP_Text>().text = isReady ? "UnReady" : "Ready";
            SetButton(btnStart,isReady);
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnLeftRoom, type =>
        {
            // Debug.Log("방 나오기 성공");
            Debug.Log($"[{DataManager.Instance.nickname}] 님이 방에서 나갔습니다.");
            Pun2Manager.Instance.LoadScene("Lobby");
            
            // PhotonNetwork.JoinLobby();
            // btnLeaveRoom.gameObject.SetActive(false);
            // btnCreateRoom.gameObject.SetActive(true);
            // uiRoomScrollview.Show();
            // loadingUI.Hide();
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnJoinedRoom, type =>
        {
            Debug.Log("방 접속 성공");
            Debug.Log($"[{PhotonNetwork.NickName}] 님이 방에 들어왔습니다.");
        });

    }

    private void SetButton(Button btn, bool isActive)
    {
        if (isActive)
        {
            btn.image.color = new Color32(112, 188, 255, 255);
            btn.interactable = true;
        }
        else
        {
            btn.image.color = new Color32(112, 188, 255, 255);
            btn.interactable = false;
        }

        btn.gameObject.SetActive(true);
    }
    
    [PunRPC]
    void SetPlayer(string nickName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DataManager.Instance.roomMemberInfos.Add(new RoomMemberInfo(nickName, false));
            Debug.Log("Player Joined!");
        }
    }
    
    [PunRPC]
    void SetReady(string nickName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RoomMemberInfo found = DataManager.Instance.roomMemberInfos.Find(x => x.nickName == nickName);
            found.isReady = true;
            Debug.Log("Player Ready!");
            SetButton(btnStart, true);
            int count = DataManager.Instance.roomMemberInfos.Count(x => !x.isReady);
            if (count == 0)
            {
                SetButton(btnStart, true);
            }
        }
    }
}
