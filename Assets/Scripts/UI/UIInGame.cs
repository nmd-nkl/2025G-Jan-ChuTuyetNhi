using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGame : MonoBehaviour {
    [Header("Game Objects")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] TextMeshProUGUI timerText;
    [Header("Timer")]
    [SerializeField] float remainingTime = 0f;
    private void Start() {
        if (pauseUI != null) {
            pauseUI.SetActive(false);
        }
    }
    private void OnEnable() {
        GameManager.OnWinGame += OnWinGame;
        GameManager.OnGameOver += OnGameOver;
    }
    private void OnDisable() {
        GameManager.OnWinGame -= OnWinGame;
        GameManager.OnGameOver -= OnGameOver;
    }
    private void Update() {
        remainingTime = Mathf.Max(remainingTime - Time.deltaTime, 0f);
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(remainingTime==0) GameManager.OnGameOverInvoke();
    }
    public void OnRestartPress() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.TogglePause();
    }
    public void OnPauseGamePress() {
        pauseUI.SetActive(true);
        GameManager.TogglePause();
    }
    public void OnResumeGamePress() {
        pauseUI.SetActive(false);
        GameManager.TogglePause();
    }
    public void OnWinGame() { 
        winUI.SetActive(true);
        GameManager.TogglePause();
    }
    public void OnGameOver() {
        gameOverUI.SetActive(true);
        GameManager.TogglePause();
    }
    public void OnExitLevel() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
