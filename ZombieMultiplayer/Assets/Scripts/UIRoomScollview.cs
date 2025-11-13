using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using TMPro;
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
    public void UpdateUI(List<RoomInfo> roomList)
    {
        // todo : 기존 UI 모두 제거
        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);

        Debug.Log(DataManager.Instance.LobbyRoomInfos.Count);
        foreach (var room in roomList)
        {
            Debug.Log(room);
            // 방이 삭제될 예정이거나 플레이어가 없는 방은 표시하지 않음
            // todo : content.GetChild(i).name 이 같으면 삭제
            if (room.RemovedFromList || room.PlayerCount == 0)
            {
                Debug.Log(room.Name);
                LobbyRoomInfo found = DataManager.Instance.LobbyRoomInfos.FirstOrDefault(p => p.id == room.Name);
                if (found != null)
                    DataManager.Instance.LobbyRoomInfos.Remove(found);
                continue;
            }

            var item = Instantiate(uiRoomCellViewPrefab, content);
            item.name = room.Name;
            item.GetComponentInChildren<TMP_Text>().text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";
            Debug.Log("룸 아이템 생성!");
        }
        Debug.Log(DataManager.Instance.LobbyRoomInfos.Count);
        // 빈 방일 경우 메시지 처리
        txtNoRoom.SetActive(content.childCount == 0);
        Debug.Log($"실제 보이는 룸 수 : {content.childCount}");
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}