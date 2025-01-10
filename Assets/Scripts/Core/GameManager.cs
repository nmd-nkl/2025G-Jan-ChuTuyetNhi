using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static event Action<bool> OnPauseStateChanged;
    public static event Action OnWinGame;
    public bool isPaused = false;

    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        OnPauseStateChanged?.Invoke(isPaused);
    }

    private int currIncorrectPipeCnt = 0;
    private void OnEnable() {
        PipeEvents.RegisterStatusChanged(HandlePipeStatusChanged);
        this.InitCount();
    }
    private void OnDisable() {
        PipeEvents.UnregisterStatusChanged(HandlePipeStatusChanged);
    }
    private void InitCount() {
        currIncorrectPipeCnt = 0;
    }
    private void HandlePipeStatusChanged(int status) {
        currIncorrectPipeCnt += status;
        Debug.Log("Current Correct Pipes: " + currIncorrectPipeCnt);
        if (currIncorrectPipeCnt == 0) {
            OnWinGame?.Invoke();
            Debug.Log("Win");
            UpdateUnlockedLevels();
        }
    }
    private void UpdateUnlockedLevels() {
        if (LevelManager.UnlockedLevels == LevelManager.CurrLevel) {
            LevelManager.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelManager.UnlockedLevels);
            PlayerPrefs.Save();
            Debug.Log(LevelManager.UnlockedLevels);
        }
    }
}
