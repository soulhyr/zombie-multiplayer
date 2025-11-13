using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UIRoomScollview : MonoBehaviour
{
    public GameObject txtNoRoom;
    public Transform content;
    public GameObject uiRoomCellViewPrefab;

    public void UpdateUI(List<RoomInfo> roomList)
    {
        Debug.Log("UpdateUI");
        foreach (var room in roomList)
        {
            Debug.Log(room);
            if (room.RemovedFromList || room.PlayerCount == 0)
            {
                Debug.Log($"룸 아이템 삭제!, room.Name: {room.Name}");
                DataManager.Instance.RemoveRoom(room.Name);
                RemoveContentItme(room.Name);
                continue;
            }

            var item = Instantiate(uiRoomCellViewPrefab, content);
            item.name = room.Name;
            string roomName = DataManager.Instance.AddRoom(room.Name, room.MaxPlayers);
            item.GetComponentInChildren<TMP_Text>().text = $"{roomName} (1/{room.MaxPlayers})";
            Debug.Log($"룸 아이템 생성!, roomName: {room.Name}");
        }
        txtNoRoom.SetActive(content.childCount == 0);
        Debug.Log($"실제 보이는 룸 수 : {content.childCount}");
    }

    private void RemoveContentItme(string roomName)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).name == roomName)
            {
                Destroy(content.GetChild(i).gameObject);
                break;
            }
        }
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}