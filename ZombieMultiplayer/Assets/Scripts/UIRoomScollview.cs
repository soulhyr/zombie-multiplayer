using System.Collections.Generic;
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

        foreach (var room in roomList)
        {
            // 방이 삭제될 예정이거나 플레이어가 없는 방은 표시하지 않음
            if (room.RemovedFromList || room.PlayerCount == 0)
                continue;

            var item = Instantiate(uiRoomCellViewPrefab, content);
            item.GetComponentInChildren<TMP_Text>().text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";
        }

        // 빈 방일 경우 메시지 처리
        txtNoRoom.SetActive(content.childCount == 0);
        Debug.Log($"실제 보이는 룸 수 : {content.childCount}");
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}