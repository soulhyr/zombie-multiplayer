using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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

    // 캐릭터 ↔ 닉네임 UI
    private Dictionary<GameObject, TMP_Text> nicknames = new Dictionary<GameObject, TMP_Text>();
    // Player ↔ 캐릭터(PhotonViewOwner) 매핑
    private Dictionary<int, GameObject> playerCharacters = new Dictionary<int, GameObject>();

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

        // 내 캐릭터 생성
        CreateLocalCharacter();

        // 이미 방에 들어와있는 다른 플레이어들 처리
        SetupExistingPlayers();
    }

    private void AddEvents()
    {
        btnBack.onClick.AddListener(() =>
        {
            Pun2Manager.Instance.LeaveRoom();
        });
        
        EventDispatcher.instance.AddEventHandler(EventDispatcher.EventType.OnLeftRoom, type =>
        {
            Pun2Manager.Instance.LoadScene("Lobby");
        });
        
        // 새 플레이어 입장 처리
        EventDispatcher.instance.AddEventHandler<Player>(EventDispatcher.EventType.OnPlayerEnteredRoom, (type, data) =>
        {
            if (data.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;

            TryLinkPlayerUI(data);
        });

        // 플레이어 나가기 처리
        EventDispatcher.instance.AddEventHandler<Player>(EventDispatcher.EventType.OnPlayerLeftRoom, (type, data) =>
        {
            RemovePlayer(data);
        });
    }


    // --- 캐릭터 생성 & 매핑 -----------------------------------------------------------

    private void CreateLocalCharacter()
    {
        var initPos = Random.insideUnitSphere * 5f;
        initPos.y = 0;
        var go = PhotonNetwork.Instantiate(characterPrefabName, initPos, Quaternion.identity);

        // 캐릭터 저장
        var pv = go.GetComponent<PhotonView>();
        playerCharacters[pv.Owner.ActorNumber] = go;

        CreateNicknameUI(go, PhotonNetwork.NickName, Color.white);
    }

    // 기존 플레이어 캐릭터 탐색
    private void SetupExistingPlayers()
    {
        foreach (var player in Pun2Manager.Instance.PlayerList)
        {
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                continue;

            TryLinkPlayerUI(player);
        }
    }


    // --- 닉네임 UI 처리 -------------------------------------------------------------

    private void TryLinkPlayerUI(Player player)
    {
        // 캐릭터가 이미 씬에서 생성된 경우 찾기
        GameObject character = FindCharacterByOwner(player);
        if (character != null)
        {
            CreateNicknameUI(character, player.NickName, Color.red);
            return;
        }

        // 아직 생성 안되었다면 지연 처리
        StartCoroutine(WaitForCharacterThenCreateUI(player));
    }

    private System.Collections.IEnumerator WaitForCharacterThenCreateUI(Player player)
    {
        GameObject character = null;

        // 최대 3초 대기
        float timer = 0f;
        while (character == null && timer < 3f)
        {
            character = FindCharacterByOwner(player);
            if (character != null)
            {
                CreateNicknameUI(character, player.NickName, Color.red);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.LogWarning($"[{player.NickName}] 캐릭터를 끝내 찾지 못했습니다.");
    }

    private GameObject FindCharacterByOwner(Player player)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            var pv = obj.GetComponent<PhotonView>();
            if (pv != null && pv.Owner.ActorNumber == player.ActorNumber)
            {
                playerCharacters[player.ActorNumber] = obj;
                return obj;
            }
        }
        return null;
    }

    private void CreateNicknameUI(GameObject character, string nickname, Color color)
    {
        if (nicknames.ContainsKey(character)) return;

        var text = Instantiate(nicknamePrefab, uiCanvas.transform);
        text.text = nickname;
        text.color = color;

        nicknames.Add(character, text);
    }

    private void RemovePlayer(Player player)
    {
        if (playerCharacters.TryGetValue(player.ActorNumber, out var character))
        {
            RemoveCharacter(character);
            playerCharacters.Remove(player.ActorNumber);
        }
    }

    void LateUpdate()
    {
        foreach (var kvp in nicknames)
        {
            var character = kvp.Key;
            var text = kvp.Value;

            if (character == null || text == null) continue;

            Vector3 worldPos = character.transform.position + Vector3.up * 2f;
            text.transform.position = mainCam.WorldToScreenPoint(worldPos);
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