using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
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
    #region Game Actions
    public static event Action<bool> OnPauseStateChanged;
    public static event Action OnWinGame;
    public static event Action OnGameOver;
    public static event Action<GameObject> OnAddStar;
    #endregion
    #region Game State
    public static bool isPaused = false;
    private static bool isGameOverInvoked = false;
    #endregion
    [Header("Timer")]
    public float remainingTime = 0f;
    public static float currRemainingTime = 0f;
    [Header("Const String")]
    public string donutName = "Donut";
    private void Start() {
        AudioManager.instance.Play(SoundEffect.BgMusic);
    }
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
    public string CalTime() {
        currRemainingTime = Mathf.Max(currRemainingTime - Time.deltaTime, 0f);
        int minutes = Mathf.FloorToInt(currRemainingTime / 60);
        int seconds = Mathf.FloorToInt(currRemainingTime % 60);
        string currTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (currRemainingTime == 0) OnGameOverInvoke();
        return currTime;
    }
    public float GetPlayedTime() {
        return remainingTime - currRemainingTime;
    }
    public static void ResetGameOverState() => isGameOverInvoked = false;
    public void ResetCountingTime() => currRemainingTime = remainingTime;
    public static void OnWinGameInvoke() => OnWinGame?.Invoke();
    public static void OnAddStarInvoke(GameObject gameObject) => OnAddStar?.Invoke(gameObject);
}
