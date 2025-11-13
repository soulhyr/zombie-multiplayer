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
        
        // if (Pun2Manager.Instance.IsMasterClient)
        // {
        //     woman1.SetActive(true);
        //     Debug.Log("nic : " + Pun2Manager.Instance.NickName);
        //     nickname1.text = Pun2Manager.Instance.NickName;
        // }
        // else
        // {
        //     woman2.SetActive(true);
        //     nickname2.text = DataManager.Instance.nickname;
        // }
        
        UpdatePlayerListUI("님이 방에 들어왔습니다.");
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
        btnStart.onClick.AddListener(() => Pun2Manager.Instance.LoadScene("Main"));
        btnReady.onClick.AddListener(() =>
        {
            isReady = !isReady;
            Debug.Log($"btnReady, isReady: {isReady}");
            btnReady.GetComponentInChildren<TMP_Text>().text = isReady ? "UnReady" : "Ready";
            SetButton(btnStart, isReady);
            btnStart.interactable = true;
            
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
        EventDispatcher.instance.AddEventHandler<Tuple<Player, Hashtable>>(EventDispatcher.EventType.OnPlayerPropertiesUpdate, (type, data) =>
        {
            // todo : 동작 확인 필요.
            Hashtable changedProps = data.Item2;
            btnStart.interactable = changedProps.ContainsKey("ready") && changedProps["ready"].Equals(true);
        });
        
        // todo : 방장 바뀌었을때 처리.
        // todo : 모두 레디하면 게임 이동.
        // todo : 게임에서 닉네임 표기.
        
        Debug.Log("AddEvents end");
    }
    
    private void UpdatePlayerListUI(string msg = "")
    {
        Debug.Log("UpdatePlayerListUI");
        woman1.SetActive(false);
        woman2.SetActive(false);
        
        nickname1.text = "UnKnow";
        nickname2.text = "UnKnow";
        
        int i = 0;
        foreach (var player in Pun2Manager.Instance.PlayerList)
        {
            if (i == 0)
            {
                woman1.SetActive(true);
                nickname1.text = player.NickName;
            }
            else
            {
                woman2.SetActive(true);
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

        btn.gameObject.SetActive(true);
    }
}
