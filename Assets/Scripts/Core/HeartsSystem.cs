using UnityEngine;
using UnityEngine.UI;

public class HeartsSystem : MonoBehaviour {
    [Header("Sprites")]
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    public static int hearts = 3;
    public static int maxHearts = 3;
    public static float healTime = 300f;
    private static float lastHealTimestamp;

    private void Awake() {
        this.GetHeartsData();
    }
    private void Update() {
        ShowUpHearts();
        CalHealHeartInGame();
    }
    private void ShowUpHearts() {
        for (int i = 0; i < heartImages.Length; i++) {
            heartImages[i].sprite = (i < hearts) ? fullHeart : emptyHeart;
        }
    }
    private void GetHeartsData() {
        hearts = PlayerPrefs.GetInt("Hearts", 3);
        lastHealTimestamp = PlayerPrefs.GetFloat("LastHealTimestamp", Time.time);
        CalculateHeartsOnStart();
    }

    private void CalculateHeartsOnStart() {
        float timePassed = Time.time - lastHealTimestamp;
        int heartsToHeal = Mathf.FloorToInt(timePassed / healTime);
        HealHeart(heartsToHeal);
        lastHealTimestamp = (hearts < maxHearts)? Time.time - (timePassed % healTime): Time.time;
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
    private void HealHeart(int _hearts) {
        hearts = Mathf.Min(maxHearts, hearts + _hearts);
        SaveHearts();
    }
    public static void LoseHeart() {
        hearts = Mathf.Max(0, hearts - 1);
        if (hearts < maxHearts) {
            SaveLastHealTimestamp(Time.time);
        }
        SaveHearts();
    }
    private static void SaveLastHealTimestamp(float _time) {
        lastHealTimestamp = _time;
        PlayerPrefs.SetFloat("LastHealTimestamp", lastHealTimestamp);
        PlayerPrefs.Save();
    }
    private static void SaveHearts() {
        PlayerPrefs.SetInt("Hearts", hearts);
        PlayerPrefs.Save();
    }
}
