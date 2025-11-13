using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomScollview : MonoBehaviour
{
    public GameObject txtNoRoom;
    public Transform content;
    public GameObject uiRoomCellViewPrefab;

    public void UpdateUI(List<RoomInfo> roomList)
    {
        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);

        Debug.Log(content.childCount);
        foreach (var room in roomList)
        {
            Debug.Log(room);
            string id = room.Name;
            int max = room.MaxPlayers;
            string roomName = DataManager.Instance.AddRoom(id, max);
            if (room.RemovedFromList || room.PlayerCount == 0)
            {
                // Debug.Log($"룸 아이템 삭제!, room.Name: {room.Name}");
                DataManager.Instance.RemoveRoom(id);
                continue;
            }
            Debug.Log("생성");
            var item = Instantiate(uiRoomCellViewPrefab, content);
            item.name = id;
            UIRoomCellView itemLogic = item.GetComponent<UIRoomCellView>();
            itemLogic.roomInfo.id = id;
            itemLogic.roomInfo.roomName = roomName;
            itemLogic.roomInfo.maxPlayers = max;
            itemLogic.roomInfo.currentPlayers = 1;
            itemLogic.SetRoomName();
        }
        Debug.Log(content.childCount);
        txtNoRoom.SetActive(content.childCount == 0);
        // Debug.Log($"실제 보이는 룸 수 : {content.childCount}");
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}