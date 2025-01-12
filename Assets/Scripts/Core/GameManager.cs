using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static event Action<bool> OnPauseStateChanged;
    public static event Action<int> OnPipeStatusChanged;
    public static event Action OnWinGame;
    public static event Action OnGameOver;

    public static bool isPaused = false;
    private int currIncorrectPipeCnt = 0;

    public static void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        OnPauseStateChanged?.Invoke(isPaused);
    }
    public static void OnPipeStatusChangedInvoke(int status) {
        OnPipeStatusChanged?.Invoke(status);
    }
    public static void OnGameOverInvoke() {
        OnGameOver?.Invoke();
    }

    private void OnEnable() {
        currIncorrectPipeCnt = 0;
        OnPipeStatusChanged += HandlePipeStatusChanged;
    }
    private void OnDisable() {
        OnPipeStatusChanged -= HandlePipeStatusChanged;
    }
    private void HandlePipeStatusChanged(int status) {
        currIncorrectPipeCnt += status;
        if (currIncorrectPipeCnt == 0) {
            OnWinGame?.Invoke();
            UpdateUnlockedLevels();
        }
    }
    private void UpdateUnlockedLevels() {
        if (LevelManager.UnlockedLevels == LevelManager.CurrLevel) {
            LevelManager.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelManager.UnlockedLevels);
            PlayerPrefs.Save();
        }
    }
}
