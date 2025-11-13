using System;
using System.Collections.Generic;

[Serializable]
public class LobbyRoomInfo
{
    public string id;              // Photon RoomName 혹은 고유 ID
    public string roomName;        // 표시용 이름
    public int currentPlayers;     // 현재 인원 수
    public int maxPlayers;         // 최대 인원 수

    public LobbyRoomInfo(string id, string roomName, int currentPlayers, int maxPlayers)
    {
        this.id = id;
        this.roomName = roomName;
        this.currentPlayers = currentPlayers;
        this.maxPlayers = maxPlayers;
    }
}
