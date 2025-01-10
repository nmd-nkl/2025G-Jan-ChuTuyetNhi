using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime = 0f;
    private void Update() {
        remainingTime = Mathf.Max(remainingTime - Time.deltaTime, 0f);
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text =  string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
