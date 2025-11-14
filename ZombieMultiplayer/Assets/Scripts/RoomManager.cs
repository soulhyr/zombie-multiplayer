using System;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public GameObject woman1;
    public GameObject woman2;
    public GameObject you1;
    public GameObject you2;
    public Button btnBack;
    public Button btnStart;
    public Button btnReady;
    public TMP_Text nickname1;
    public TMP_Text nickname2;

    public UILoading uiLoadingPrefab;
    private UILoading loadingUI;
    
    private bool isReady;
    
    void Awake()
    {
        loadingUI = Instantiate(uiLoadingPrefab);
        loadingUI.Show();
        
        Init();
        AddEvents();
        
        loadingUI.Hide();
    }

    void OnDestroy() => ResetEvent();
    
    private void Init()
    {
        Pun2Manager.Instance.Init();
        
        SetButton(btnStart, Pun2Manager.Instance.IsMasterClient);
        SetButton(btnReady, !Pun2Manager.Instance.IsMasterClient);
        
        btnStart.interactable = false;
        btnStart.gameObject.SetActive(Pun2Manager.Instance.IsMasterClient);
        btnReady.gameObject.SetActive(!Pun2Manager.Instance.IsMasterClient);
        
        UpdatePlayerListUI("님이 방에 들어왔습니다.");
        
        if (Pun2Manager.Instance.CurrentRoom != null)
        {
            Debug.Log("Room Name: " + Pun2Manager.Instance.CurrentRoom.Name);
        }
        
        if (Pun2Manager.Instance.IsConnectedAndReady)
        {
            Debug.Log("Ready to sync scene");
        }
        else
        {
            Debug.Log("Not connected or not in room");
        }


    }

    private void ResetEvent()
    {
        btnBack.onClick.RemoveAllListeners();
        btnStart.onClick.RemoveAllListeners();
        btnReady.onClick.RemoveAllListeners();

        EventDispatcher.instance.RemoveAllEventHandlers();
    }
    
    private void AddEvents()
    {
        ResetEvent();
        
        btnBack.onClick.AddListener(() =>
        {
            Debug.Log("btnBack");
            Pun2Manager.Instance.LeaveRoom();
        });
        btnStart.onClick.AddListener(() =>
        {
            if (!Pun2Manager.Instance.IsMasterClient)
            {
                Debug.Log("발생할 수 없지만 예방한건데 걸렸으니 큰일!!");
                return;
            }
            Pun2Manager.Instance.AutomaticallySyncScene = true;
            Debug.Log(Pun2Manager.Instance.AutomaticallySyncScene);
            Pun2Manager.Instance.LoadScene("Main");
        });
        btnReady.onClick.AddListener(() =>
        {
            isReady = !isReady;
            Debug.Log($"btnReady, isReady: {isReady}");
            btnReady.GetComponentInChildren<TMP_Text>().text = isReady ? "UnReady" : "Ready";
            SetButton(btnStart, isReady);
            
            Hashtable props = new Hashtable();
            props["ready"] = isReady;
            Pun2Manager.Instance.SetMyProperties(props);
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnLeftRoom, type =>
        {
            Debug.Log($"[{Pun2Manager.Instance.NickName}] 님이 방에서 나갔습니다.");
            Pun2Manager.Instance.LoadScene("Lobby");
            
            Debug.Log("LoadScene End");
        });
        EventDispatcher.instance.AddEventHandler<Player>(EventDispatcher.EventType.OnPlayerEnteredRoom, (type, data) => 
        {
            Debug.Log($"{data.NickName}님이 방에 들어왔습니다!");
            UpdatePlayerListUI();
        });
        EventDispatcher.instance.AddEventHandler<Player>(EventDispatcher.EventType.OnPlayerLeftRoom, (type, data) =>
        {
            Debug.Log($"{data.NickName}님이 방에서 나갔습니다!");
            UpdatePlayerListUI();
        });
        EventDispatcher.instance.AddEventHandler<(Player, Hashtable)>(EventDispatcher.EventType.OnPlayerPropertiesUpdate, (type, data) =>
        {
            Hashtable changedProps = data.Item2;
            SetButton(btnStart,changedProps.ContainsKey("ready") && changedProps["ready"].Equals(true));
        });
        EventDispatcher.instance.AddEventHandler<Player>(EventDispatcher.EventType.OnMasterClientSwitched, (type, data) =>
        {
            SetButton(btnStart, Pun2Manager.Instance.IsMasterClient);
            SetButton(btnReady, !Pun2Manager.Instance.IsMasterClient);
        });
    }
    
    private void UpdatePlayerListUI(string msg = "")
    {
        Debug.Log("UpdatePlayerListUI");
        woman1.SetActive(false);
        woman2.SetActive(false);
        you1.SetActive(false);
        you2.SetActive(false);
        
        nickname1.text = "UnKnow";
        nickname2.text = "UnKnow";
        
        int i = 0;
        foreach (var player in Pun2Manager.Instance.PlayerList)
        {
            if (i == 0)
            {
                woman1.SetActive(true);
                if (Pun2Manager.Instance.NickName == player.NickName)
                    you1.SetActive(true);
                nickname1.text = player.NickName;
            }
            else
            {
                woman2.SetActive(true);
                if (Pun2Manager.Instance.NickName == player.NickName)
                    you2.SetActive(true);
                nickname2.text = player.NickName;
            }

            if (msg.Length > 0)
            {
                Debug.Log($"[{player.NickName}] {msg}");
            }
            i++;
        }
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

        btn.gameObject.SetActive(isActive);
    }
}
