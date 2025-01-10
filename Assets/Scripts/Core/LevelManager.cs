using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public Button[] levelButtons;
    public static int CurrLevel = 0;
    public static int UnlockedLevels = 0;
    private void Start() {
        Debug.Log(UnlockedLevels);
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelButtons.Length; i++) {
            if(UnlockedLevels >= i) {
                levelButtons[i].interactable = true;
            }
        }
    }
}
