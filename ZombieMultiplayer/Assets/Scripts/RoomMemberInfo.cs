using System;

[Serializable]
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