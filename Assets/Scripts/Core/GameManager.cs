using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static event Action<bool> OnPauseStateChanged;
    public static event Action OnWinGame;
    public static event Action OnGameOver;

    public static bool isPaused = false;
    private static bool isGameOverInvoked = false;
    [Header("Timer")]
    public float remainingTime = 0f;
    public static float currRemainingTime = 0f;

    #region singleton
    public static GameManager instance { get; private set; }
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion
    public static void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        OnPauseStateChanged?.Invoke(isPaused);
    }
    public static void OnGameOverInvoke() {
        if (!isGameOverInvoked) {
            isGameOverInvoked = true;
            OnGameOver?.Invoke();
        }
    }
    public static void ResetGameOverState() {
        isGameOverInvoked = false;
    }
    public static void OnWinGameInvoke() {
        OnWinGame?.Invoke();
    }
    public string CalTime() {
        currRemainingTime = Mathf.Max(currRemainingTime - Time.deltaTime, 0f);
        int minutes = Mathf.FloorToInt(currRemainingTime / 60);
        int seconds = Mathf.FloorToInt(currRemainingTime % 60);
        string currTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (currRemainingTime == 0) OnGameOverInvoke();
        return currTime;
    }
    public void ResetCountingTime() {
        currRemainingTime = remainingTime;
    }
}
