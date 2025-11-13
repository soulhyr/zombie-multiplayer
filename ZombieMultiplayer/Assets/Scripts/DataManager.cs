using System;
using System.Collections.Generic;
using UnityEngine;


public class RoomMemberInfo
{
    public string nickName;
    public bool isReady;

    public RoomMemberInfo(string nickName, bool isReady)
    {
        this.nickName = nickName;
        this.isReady = isReady;
    }
}

public class LobbyRoomInfo
{
    public string id;
    public string roomName;
    public List<RoomMemberInfo> roomMembers;
    
    public LobbyRoomInfo(string id, string nickName)
    {
        this.id = id;
        this.roomName = DateTime.Now.ToString("yyyyMMddHHmmss");
        this.roomMembers = new List<RoomMemberInfo>();
        roomMembers.Add(new RoomMemberInfo(nickName, true));
    }
}

public class DataManager
{
    private static readonly DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    public string nickname = string.Empty;
    public List<RoomMemberInfo> players = new List<RoomMemberInfo>();
    public List<LobbyRoomInfo> LobbyRoomInfos = new List<LobbyRoomInfo>();

    private DataManager() { }
}