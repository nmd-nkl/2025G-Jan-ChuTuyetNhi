using DG.Tweening;
using TMPro;
using UnityEngine;

public class HeartsSystem : MonoBehaviour {
    [Header("Text")]
    [SerializeField] TextMeshProUGUI heartsCountTxt;
    [SerializeField] TextMeshProUGUI healTimeTxt;
    [Header("UI")]
    [SerializeField] RectTransform HeartIcon;
    [SerializeField] RectTransform AdsButton;

    [Header("Data")]
    public int maxHearts = 3;
    public float healTime = 300f;

    public static int hearts = 3;
    public static float timeLeft = 0;
    private static float lastHealTimestamp;

    #region Singleton & Awake
    public static HeartsSystem instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    private void Start() {
        GetHeartsData();
    }
    private void Update() {
        ShowUpHearts();
        CalHealHeartInGame();
    }

    private void ShowUpHearts() {
        heartsCountTxt.text = hearts.ToString();

        if (hearts < maxHearts) {
            if(hearts==0) AdsButton.gameObject.SetActive(true);
            timeLeft = healTime - (Time.time - lastHealTimestamp);
            timeLeft = Mathf.Max(0, timeLeft);
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            healTimeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        } else {
            healTimeTxt.text = "FULL";
            AdsButton.gameObject.SetActive(false);
        }
    }

    private void GetHeartsData() {
        hearts = PlayerPrefs.GetInt("Hearts", maxHearts);
        string lastHealStr = PlayerPrefs.GetString("LastHealTimestamp", System.DateTime.UtcNow.ToString());
        lastHealTimestamp = (float)(System.DateTime.UtcNow - System.DateTime.Parse(lastHealStr)).TotalSeconds;
        CalculateHeartsOnStart();
    }

    private void CalculateHeartsOnStart() {
        float timePassed = (float)(System.DateTime.UtcNow - System.DateTime.Parse(PlayerPrefs.GetString("LastHealTimestamp", System.DateTime.UtcNow.ToString()))).TotalSeconds;
        if (timePassed < 0) timePassed = 0;
        int heartsToHeal = Mathf.FloorToInt(timePassed / healTime);

        if (heartsToHeal > 0) {
            HealHeart(heartsToHeal);
        }

        if (hearts < maxHearts) {
            lastHealTimestamp = Time.time - (timePassed % healTime);
        } else {
            lastHealTimestamp = Time.time;
        }

        SaveLastHealTimestamp(lastHealTimestamp);
    }
    private void CalHealHeartInGame() {
        if (hearts < maxHearts) {
            float timeSinceLastHeal = Time.time - lastHealTimestamp;
            if (timeSinceLastHeal >= healTime) {
                HealHeart(1);
                SaveLastHealTimestamp(Time.time);
            }
        }
    }

    public void HealHeart(int _hearts) {
        if (hearts < maxHearts && _hearts != 0) LvUIManager.instance.AnimHeart(HeartIcon);
        hearts = Mathf.Min(maxHearts, hearts + _hearts);
        SaveHearts();
    }

    public void LoseHeart() {
        hearts = Mathf.Max(0, hearts - 1);
        SaveHearts();
        SaveLastHealTimestamp(Time.time);
    }
    private static void SaveLastHealTimestamp(float _time) {
        lastHealTimestamp = _time;
        PlayerPrefs.SetString("LastHealTimestamp", System.DateTime.UtcNow.ToString()); 
        PlayerPrefs.Save();
    }
    private static void SaveHearts() {
        PlayerPrefs.SetInt("Hearts", hearts);
        PlayerPrefs.Save();
    }
    private void OnDisable() {
        DOTween.KillAll();
    }
    public string GetHealTimeTxt() {
        return healTimeTxt.text;
    }
}
