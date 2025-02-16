using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class TriggerJoke : MonoBehaviour {
    public static GameObject pauseButton;
    public Light2D globalLight;
    public Light2D spotLight;
    public GameObject jokeText1;
    public GameObject jokeText2;
    GameObject donut;
    private bool isJoking = false;

    private void Start() {
        GameObject canvasUI = GameObject.Find("Canvas UI");
        Transform pauseButtonTransform = canvasUI.transform.Find("Pause Button");
        pauseButton = pauseButtonTransform.gameObject;

        GameObject globalLightObj = GameObject.Find("Global Light 2D");
        if (globalLightObj != null) {
            globalLight = globalLightObj.GetComponent<Light2D>();
        }

        donut = GameObject.Find("Donut");

        jokeText1.SetActive(false);
        jokeText2.SetActive(false);
        isJoking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isJoking) return;

        isJoking = true;
        pauseButton.SetActive(false);
        GameManager.TogglePause();

        if (globalLight != null) {
            globalLight.intensity = 0.5f;
        }

        if (spotLight != null) {
            spotLight.gameObject.SetActive(true);
            spotLight.intensity = 0.5f;
            spotLight.transform.position = donut.transform.position;
        }

        jokeText1.transform.position = donut.transform.position + new Vector3(5f, 0, 0);
        jokeText2.transform.position = jokeText1.transform.position - new Vector3(-1f, 1f, 0);
        jokeText1.SetActive(true);
        DOVirtual.DelayedCall(2.5f, () => {
            jokeText1.SetActive(false);
            jokeText2.SetActive(true);
        });

        DOVirtual.DelayedCall(5.5f, () => {
            if (globalLight != null) {
                globalLight.intensity = 1f;
            }
            if (spotLight != null) {
                spotLight.gameObject.SetActive(false);
            }
            jokeText2.SetActive(false);
            GameManager.TogglePause();
            gameObject.SetActive(false);
        });
    }
}
