using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIRoomScollview : MonoBehaviour
{
    public GameObject txtNoRoom;
    public Transform content;
    public GameObject uiRoomCellViewPrefab;

    public void AddEvents()
    {
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>(EventDispatcher.EventType.OnRoomListUpdate, (type, data) =>
        {
            Debug.Log(data.Count);
            txtNoRoom.SetActive(data.Count == 0);
        });
    }
    public void UpdateUI()
    {
        Debug.Log("UpdateUI");
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}