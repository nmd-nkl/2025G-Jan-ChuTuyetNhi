using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class TutorialForUser : MonoBehaviour {
    public GameObject spot1;
    public GameObject spot2;
    public GameObject textStep1;
    public GameObject textStep2;
    public Light2D spotLight;
    public Light2D globalLight;
    public static int currTutorialStep = 1;
    public float moveDuration = 1.5f;
    #region singleton
    public static TutorialForUser instance { get; private set; }
    private void Awake() {
        if (instance == null) {
            instance = this;
            currTutorialStep = 1;
            if(globalLight==null) {
                GameObject globalLightObj = GameObject.Find("Global Light 2D");
                if (globalLightObj != null) {
                    globalLight = globalLightObj.GetComponent<Light2D>();
                }
            }
        } 
    }
    #endregion
    public void Step1() {
        textStep1.SetActive(true);
        spotLight.gameObject.SetActive(true);
        globalLight.intensity = 0.5f;
        spotLight.intensity = 0.5f;
        transform.position = spot1.transform.position;
    }
    public void DoneStep1() {
        textStep1.SetActive(false);
        spotLight.gameObject.SetActive(false);
        globalLight.intensity = 1f;
        spotLight.intensity = 0f;
        currTutorialStep++;
        Step2();
    }
    public void Step2() {
        spot1.SetActive(false);
        spot2.SetActive(true);
        textStep2.SetActive(true);
        spotLight.gameObject.SetActive(true);
        globalLight.intensity = 0.5f;
        spotLight.intensity = 0.5f;
        transform.DOMove(spot2.transform.position, moveDuration).SetEase(Ease.InOutQuad).SetUpdate(true);
    }
    public void DoneStep2() {
        spot2.gameObject.SetActive(false);
        textStep2.SetActive(false);
        spotLight.gameObject.SetActive(false);
        globalLight.intensity = 1f;
        spotLight.intensity = 0f;
        currTutorialStep++;
        DOVirtual.DelayedCall(1f, () => {
            GameManager.TogglePause();
            gameObject.SetActive(false);
            DOTween.KillAll();
        });
    }
}