using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas uiCanvas;
    public TMP_Text nicknamePrefab;
    public Button btnBack;
    
    public UILoading uiLoadingPrefab;
    private UILoading loadingUI;

    private Camera mainCam;
    private readonly string characterPrefabName = "Woman";
    private Dictionary<GameObject, TMP_Text> nicknames = new Dictionary<GameObject, TMP_Text>();
    
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
        if (uiCanvas == null)
        {
            Debug.LogError("씬에 Canvas가 필요합니다!");
            return;
        }

        mainCam = Camera.main;
        CreateCharacterForPlayer();
        foreach (var player in Pun2Manager.Instance.PlayerList)
        {
            Debug.Log($"[{player.NickName}] 님이 전투에 참여했습니다.");
        }
    }

    private void AddEvents()
    {
        btnBack.onClick.AddListener(() =>
        {
            // Pun2Manager.Instance.LoadScene("Lobby");
            Pun2Manager.Instance.LeaveRoom();
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnLeftRoom, type =>
        {
            Debug.Log($"[{Pun2Manager.Instance.NickName}] 님이 전투에서 나갔습니다.");
            Pun2Manager.Instance.LoadScene("Lobby");
            
            Debug.Log("LoadScene End");
        });
    }

    private void ResetEvent()
    {
        btnBack.onClick.RemoveAllListeners();

        EventDispatcher.instance.RemoveAllEventHandlers();
    }

    private void CreateCharacterForPlayer()
    {
        var initPos = Random.insideUnitSphere * 5f;
        initPos.y = 0;
        var go = PhotonNetwork.Instantiate(characterPrefabName, initPos, Quaternion.identity);

        // 닉네임 생성
        var text = Instantiate(nicknamePrefab, uiCanvas.transform);
        text.text = PhotonNetwork.NickName;
        nicknames.Add(go, text);
    }

    void LateUpdate()
    {
        foreach (var kvp in nicknames)
        {
            var charObj = kvp.Key;
            var text = kvp.Value;

            if (charObj == null || text == null) continue;

            Vector3 screenPos = mainCam.WorldToScreenPoint(charObj.transform.position + Vector3.up * 2f);
            text.transform.position = screenPos;
        }
    }

    public void RemoveCharacter(GameObject character)
    {
        if (nicknames.TryGetValue(character, out TMP_Text text))
        {
            Destroy(text.gameObject);
            nicknames.Remove(character);
        }
    }
}