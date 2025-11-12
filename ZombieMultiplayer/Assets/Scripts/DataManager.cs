public class DataManager
{
    private static readonly DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    public string nickname = string.Empty;

    private DataManager() { }

    public void SetName(string name) => nickname = name;
}