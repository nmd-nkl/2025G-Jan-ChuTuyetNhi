using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour {
    public ButtonLvObj[] levelButtons;
    public static int CurrLevel = 0;

    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;

    public static int UnlockedLevels {
        get => PlayerPrefs.GetInt("UnlockedLevels", 1);
        set {
            PlayerPrefs.SetInt("UnlockedLevels", value);
            PlayerPrefs.Save();
        }
    }
    public void OnClickLevelMenu(int level) {
        if (HeartsSystem.hearts != 0) {
           EnterGameLv(level);
        } else {
            CantEnterGameLv();
        }
    }
    private void EnterGameLv(int level) {
        AudioManager.instance.PlaySound("selectUI");
        CurrLevel = level;
        GameManager.instance.ResetCountingTime();
        SceneManager.LoadScene("InGame");
    }
    private void CantEnterGameLv() {
        LvUI.instance.WarnZeroInfoEnable();
    }
    private void Start() {
        UpdateLevelStars();
        for (int i = 0; i < levelButtons.Length; i++) {
            if (UnlockedLevels >= i+1) {
                levelButtons[i].levelButton.interactable = true;
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
    private void UpdateLevelStars() {
        for (int i = 0; i < levelButtons.Length; i++) {
            int starsEarned = PlayerPrefs.GetInt("LevelStars_" + (i + 1), 0);
            levelButtons[i].UpdateStars(starsEarned, fullStar, emptyStar);
        }
    }
}
