using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static event Action<bool> OnPauseStateChanged;
    private bool isPaused = false;

    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        OnPauseStateChanged?.Invoke(isPaused);
    }
}
