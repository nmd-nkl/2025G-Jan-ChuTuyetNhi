using UnityEngine;
using UnityEngine.UI;

public class StarsSystems : MonoBehaviour {
    [Header("Sprites")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;

    [Header("Values")]
    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float threeStarsPer = 0.67f;
    [SerializeField] private float twoStarsPer = 0.33f;
    public static int stars = 3;

    private void OnEnable() {
        this.CalculateStars();
        this.ShowUpHearts();
    }

    private void ShowUpHearts() {
        for (int i = 0; i < starImages.Length; i++) {
            starImages[i].sprite = (i < stars) ? fullStar : emptyStar;
        }
    }

    public void CalculateStars() {
        float currRemainingTime = GameManager.currRemainingTime;
        float timeRatio = Mathf.Clamp01(currRemainingTime / maxTime);
        if (timeRatio >= threeStarsPer) {
            stars = 3;
        } else if (timeRatio >= twoStarsPer) {
            stars = 2;
        } else if (timeRatio > 0f) {
            stars = 1;
        } else {
            stars = 0;
        }
    }
}
