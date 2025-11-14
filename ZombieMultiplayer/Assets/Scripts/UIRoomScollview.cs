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
        foreach (var room in roomList)
        {
            Debug.Log(room);
            string id = room.Name;
            int max = room.MaxPlayers;
            string roomName = DataManager.Instance.AddRoom(id, max);
            Transform child = null;
            for (int i = 0; i < content.childCount; i++)
            {
                if (content.GetChild(i).name == id)
                {
                    Debug.Log("search:" + content.GetChild(i).name);
                    child = content.GetChild(i);
                    break;
                }
            }
            
            if (room.RemovedFromList || room.PlayerCount == 0)
            {
                if (child != null)
                {
                    Debug.Log("delete object");
                    DestroyImmediate(child.gameObject);
                }

                Debug.Log("delete data");
                DataManager.Instance.RemoveRoom(id);
                continue;
            }

            if (child != null)
            {
                Debug.Log("갱신");
                UIRoomCellView logic = child.GetComponent<UIRoomCellView>();
                logic.roomInfo.maxPlayers = max;
                logic.roomInfo.currentPlayers = room.PlayerCount;
                logic.SetRoomName();
            }
            else
            {
                Debug.Log("생성");
                var item = Instantiate(uiRoomCellViewPrefab, content);
                item.name = id;
                UIRoomCellView logic = item.GetComponent<UIRoomCellView>();
                logic.roomInfo.id = id;
                logic.roomInfo.roomName = roomName;
                logic.roomInfo.maxPlayers = max;
                logic.roomInfo.currentPlayers = 1;
                logic.SetRoomName();
            }
        }
        
        txtNoRoom.SetActive(content.childCount == 0);
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}