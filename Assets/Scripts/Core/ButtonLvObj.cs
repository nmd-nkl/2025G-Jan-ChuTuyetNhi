using UnityEngine;
using UnityEngine.UI;

public class ButtonLvObj : MonoBehaviour {
    public Button levelButton;
    public Image[] stars;
    private void Awake() {
        levelButton = GetComponent<Button>();
    }
    public void UpdateStars(int starCount, Sprite fullStar, Sprite emptyStar) {
        for (int i = 0; i < stars.Length; i++) {
            stars[i].sprite = (i < starCount) ? fullStar : emptyStar;
        }
    }
}
