using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayUI : MonoBehaviour {
    public void OnClickPlayMenu() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        ResetPlayerPrefs();
        SceneManager.LoadScene(1);
    }
    public void ResetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs has been reset.");
    }
}
