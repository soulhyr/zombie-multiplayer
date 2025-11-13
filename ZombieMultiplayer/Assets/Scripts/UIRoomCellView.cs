using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomCellView : MonoBehaviour
{
    public TMP_Text txtName;
    public Button btn;
    public LobbyRoomInfo roomInfo;

    void Awake() => AddEvents();
    
    public void SetRoomName() => txtName.text = $"{roomInfo.roomName} ({roomInfo.currentPlayers}/{roomInfo.maxPlayers})";

    private void AddEvents()
    {
        btn.onClick.AddListener(() =>
        {
            Debug.Log($"room name : {name}");
            Pun2Manager.Instance.JoinRoom(name);
        });
    }
}