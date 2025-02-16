using UnityEditor;
using UnityEngine;

public class PlayerPrefsResetTool : EditorWindow {
    [MenuItem("Tools/PlayerPrefs Reset Tool")]
    public static void ShowWindow() {
        GetWindow<PlayerPrefsResetTool>("PlayerPrefs Reset");
    }

    private void OnGUI() {
        GUILayout.Label("Reset PlayerPrefs", EditorStyles.boldLabel);

        if (GUILayout.Button("Reset PlayerPrefs", GUILayout.Height(30))) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs đã được reset!");
            
        }
    }
}
