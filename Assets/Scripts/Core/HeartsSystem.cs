using DG.Tweening;
using TMPro;
using UnityEngine;

public class HeartsSystem : MonoBehaviour {
    [Header("Text")]
    [SerializeField] TextMeshProUGUI heartsCountTxt;
    [SerializeField] TextMeshProUGUI healTimeTxt;
    [Header("UI")]
    [SerializeField] RectTransform HeartIcon;

    [Header("Data")]
    public int maxHearts = 3;
    public float healTime = 300f;

    public static int hearts = 3;
    public static float timeLeft = 0;
    private static float lastHealTimestamp;

    #region Singleton & Awake
    public static HeartsSystem instance;
    private void Awake() {
        GetHeartsData();
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    private void Update() {
        ShowUpHearts();
        CalHealHeartInGame();
    }

    private void ShowUpHearts() {
        heartsCountTxt.text = hearts.ToString();

        if (hearts < maxHearts) {
            timeLeft = healTime - (Time.time - lastHealTimestamp);
            timeLeft = Mathf.Max(0, timeLeft);
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            healTimeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        } else {
            healTimeTxt.text = "Full";
        }
    }

    private void GetHeartsData() {
        hearts = PlayerPrefs.GetInt("Hearts", maxHearts);
        lastHealTimestamp = PlayerPrefs.GetFloat("LastHealTimestamp", Time.time);
        CalculateHeartsOnStart();
    }

    private void CalculateHeartsOnStart() {
        float timePassed = Time.time - lastHealTimestamp;
        int heartsToHeal = Mathf.FloorToInt(timePassed / healTime);
        HealHeart(heartsToHeal);

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

    private void HealHeart(int _hearts) {
        if(hearts < maxHearts && _hearts != 0) AnimHeart();
        hearts = Mathf.Min(maxHearts, hearts + _hearts);
        SaveHearts();
    }

    public void LoseHeart() {
        hearts = Mathf.Max(0, hearts - 1);
        SaveHearts();
        if (hearts < maxHearts) {
            SaveLastHealTimestamp(Time.time);
        }
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

    public float scaleDuration = 0.2f;
    public float moveDuration = 0.5f;
    public float starbounceHeight = 10f;
    public void AnimHeart() {
        Sequence heartSequence = DOTween.Sequence();
        heartSequence.Append(HeartIcon.transform.DOScale(0.8f, scaleDuration * 0.5f))
                    .Append(HeartIcon.transform.DOScale(1.2f, scaleDuration))
                    .Append(HeartIcon.transform.DOScale(1f, scaleDuration * 0.5f))
                    .Join(HeartIcon.transform.DOLocalMoveY(starbounceHeight, moveDuration * 0.3f).SetRelative().SetEase(Ease.OutQuad))
                    .Append(HeartIcon.transform.DOLocalMoveY(-starbounceHeight, moveDuration * 0.3f).SetRelative().SetEase(Ease.InQuad));
    }
    private void OnDisable() {
        DOTween.KillAll();
    }
    public string GetHealTimeTxt() {
        return healTimeTxt.text;
    }
}
