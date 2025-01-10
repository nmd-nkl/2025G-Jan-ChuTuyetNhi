using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayUI : MonoBehaviour {
    public void OnClickPlayMenu() {
        ResetPlayerPrefs();
        SceneManager.LoadScene(1);
    }
    public void OnClickLevelMenu(int level) {
        LevelManager.CurrLevel = level-1;
        SceneManager.LoadScene("Level " + level);
    }

    public void ResetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs has been reset.");
    }
}
