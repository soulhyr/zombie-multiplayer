using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public class OpenSaveFolder
{
    [MenuItem("Tools/Open Save Folder")]
    public static void OpenSaveFolderInExplorer()
    {
        string path = Application.persistentDataPath;

        // 경로가 없으면 경고
        if (string.IsNullOrEmpty(path))
        {
            UnityEngine.Debug.LogWarning("Application.persistentDataPath 경로가 없습니다.");
            return;
        }

        // Windows, macOS, Linux 모두 지원
        OpenFolder(path);
    }

    private static void OpenFolder(string path)
    {
#if UNITY_EDITOR_WIN
        Process.Start("explorer.exe", path.Replace("/", "\\"));
#elif UNITY_EDITOR_OSX
        Process.Start("open", path);
#elif UNITY_EDITOR_LINUX
        Process.Start("xdg-open", path);
#else
        UnityEngine.Debug.LogWarning("플랫폼을 지원하지 않습니다: " + Application.platform);
#endif
    }
}