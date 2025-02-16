using UnityEngine;

public class TriggerStopForGuide : MonoBehaviour {
    public static bool isStartTutorial = false;
    public static GameObject pauseButton;
    public void Start() {
        GameObject canvasUI = GameObject.Find("Canvas UI");
        Transform pauseButtonTransform = canvasUI.transform.Find("Pause Button");
        pauseButton = pauseButtonTransform.gameObject;
        isStartTutorial =false;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        isStartTutorial = true;
        pauseButton.SetActive(false);
        GameManager.TogglePause();
        TutorialForUser.instance.Step1();
        gameObject.SetActive(false);
    }
}
