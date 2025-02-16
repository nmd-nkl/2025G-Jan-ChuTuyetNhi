using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
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
        levelButtons[level-1].gameObject.transform.DOScale(0.95f, 0.1f).OnComplete(() => {
            levelButtons[level - 1].gameObject.transform.DOScale(1f, 0.2f).OnComplete(() => {
                if (HeartsSystem.hearts != 0) EnterGameLv(level);
                else CantEnterGameLv();
            });
        });
    }
    public void OnMoreHeartsPress() {
        LvUIManager.instance.WarnZeroInfoEnable();
    }
    public void OnAdsPress() {
        Debug.Log("Setup Ads");
    }
    private void EnterGameLv(int level) {
        DOTween.KillAll();
        AudioManager.instance.Play(SoundEffect.EnterLvMusic);
        CurrLevel = level;
        SceneManager.LoadScene("InGame");
    }
    private void CantEnterGameLv() {
        LvUIManager.instance.WarnZeroInfoEnable();
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
