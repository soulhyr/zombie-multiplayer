using System.Collections.Generic;


public class ReadyInfo
{
    public string nickName;
    public bool isReady;

    public ReadyInfo(string nickName, bool isReady)
    {
        this.nickName = nickName;
        this.isReady = isReady;
    }
}

public class DataManager
{
    private static readonly DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    public string nickname = string.Empty;
    public List<ReadyInfo> players = new List<ReadyInfo>();

    private DataManager() { }

    public void SetName(string name) => nickname = name;
}