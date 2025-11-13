using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager
{
    private bool useJson = false;
    private static readonly DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    public string nickname = string.Empty;
    public List<RoomMemberInfo> roomMemberInfos = new List<RoomMemberInfo>();
    // public List<LobbyRoomInfo> lobbyRooms = new List<LobbyRoomInfo>();
    public List<LobbyRoomInfo> rooms = new List<LobbyRoomInfo>();
    
    private readonly string filePath = Path.Combine(Application.persistentDataPath, "lobby_rooms.json");

    private DataManager() { }

    private void SaveRooms(List<LobbyRoomInfo> lobbyRooms)
    {
        if (useJson)
        {
            string json = JsonConvert.SerializeObject(lobbyRooms, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Debug.Log(json);
            Debug.Log($"[LobbyDataManager] 방 목록 저장 완료: {filePath}");
        }
    }

    public string AddRoom(string id, int maxPlayers)
    {
        string strName = DateTime.Now.ToString("yyyyMMddHHmmss");
        LobbyRoomInfo data = new LobbyRoomInfo(id, strName, 1, maxPlayers);
        if (useJson)
        {
            List<LobbyRoomInfo> list = LoadRooms();
            
            if (list == null)
                list = new List<LobbyRoomInfo>();
            list.Add(data);
            SaveRooms(list);
        }
        else
        {
            rooms.Add(data);
        }
        return strName;
    }

    public void UpdateRoom(string id, int curPlayers)
    {
        if (useJson)
        {
            List<LobbyRoomInfo> list = LoadRooms();
            if (list == null)
                list = new List<LobbyRoomInfo>();
            list.Find(x => x.id == id).currentPlayers = curPlayers;
            SaveRooms(list);
        }
        else
        {
            rooms.Find(x => x.id == id).currentPlayers = curPlayers;
        }
    }

    public void RemoveRoom(string id)
    {
        if (useJson)
        {
            List<LobbyRoomInfo> list = LoadRooms();
            if (list == null)
                list = new List<LobbyRoomInfo>();
            list.Remove(list.Find(x => x.id == id));
            SaveRooms(list);
        }
        else
        {
            rooms.Remove(rooms.Find(x => x.id == id));
        }
    }

    public List<LobbyRoomInfo> LoadRooms()
    {
        if (useJson)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log("[LobbyDataManager] 저장된 방 목록이 없습니다.");
                return null;
            }

            string json = File.ReadAllText(filePath);
            List<LobbyRoomInfo> lobbyRooms = JsonConvert.DeserializeObject<List<LobbyRoomInfo>>(json);
            Debug.Log($"[LobbyDataManager] 방 목록 불러오기 완료: {lobbyRooms.Count}개 방");
            return lobbyRooms;
        }
        else
        {
            return rooms;
        }
    }
}