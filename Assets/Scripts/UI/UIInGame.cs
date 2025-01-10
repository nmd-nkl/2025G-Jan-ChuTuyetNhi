using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGame : MonoBehaviour {
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameManager gameManager;
    private void Start() {
        if (pauseUI != null) {
            pauseUI.SetActive(false);
        }
    }
    private void OnEnable() {
        GameManager.OnWinGame += OnWinGame;
    }
    private void OnDisable() {
        GameManager.OnWinGame -= OnWinGame;
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
    public void OnWinGame() { 
        winUI.SetActive(true);
        gameManager.TogglePause();
    }
    public void OnExitLevel() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
