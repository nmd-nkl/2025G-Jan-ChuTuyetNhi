using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameManager gameManager;
    private void Start() {
        if (pauseUI != null) {
            pauseUI.SetActive(false);
        }
    }
    public void OnRestartPress() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.TogglePause();
    }
    public void OnPauseGamePress() {
        pauseUI.SetActive(true);
        gameManager.TogglePause();
    }
    public void OnResumeGamePress() {
        pauseUI.SetActive(false);
        gameManager.TogglePause();
    }
}
