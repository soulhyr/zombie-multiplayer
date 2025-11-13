using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomCellView : MonoBehaviour
{
    public Button btn;
    void Awake() => btn.onClick.AddListener(() => Pun2Manager.Instance.JoinRoom(this.name));
}