using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PlayerPrefsResetOnPlay
{
    static PlayerPrefsResetOnPlay()
    {
        EditorApplication.playModeStateChanged += ResetPlayerPrefsOnPlayMode;
    }

    private static void ResetPlayerPrefsOnPlayMode(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs have been reset for Play Mode");
        }
    }
}
