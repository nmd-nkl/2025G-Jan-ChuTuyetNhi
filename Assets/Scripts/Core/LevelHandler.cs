using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour {
    public Button[] levelButtons;
    public static int CurrLevel = 0;
    public static int UnlockedLevels {
        get => PlayerPrefs.GetInt("UnlockedLevels", 1);
        set {
            PlayerPrefs.SetInt("UnlockedLevels", value);
            PlayerPrefs.Save();
        }
    }
    public void OnClickLevelMenu(int level) {
        CurrLevel = level;
        GameManager.instance.ResetCountingTime();
        SceneManager.LoadScene("InGame");
    }
    private void Start() {
        for (int i = 0; i < levelButtons.Length; i++) {
            if (UnlockedLevels >= i+1) {
                levelButtons[i].interactable = true;
            }
        }
        GameManager.ResetGameOverState();
    }
    public static void UnlockNextLevel() {
        if (UnlockedLevels == CurrLevel) {
            UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", UnlockedLevels);
            PlayerPrefs.Save();
        }
    }
}
