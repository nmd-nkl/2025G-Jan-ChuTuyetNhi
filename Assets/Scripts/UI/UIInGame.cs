using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIInGame : MonoBehaviour {
    [Header("UI")]
    [SerializeField] GameObject pauseButton;
    [SerializeField] RectTransform pauseUI;
    [SerializeField] RectTransform winUI;
    [SerializeField] RectTransform gameOverUI;
    [SerializeField] RectTransform[] Stars;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI recordText;
    [SerializeField] private float bounceHeight = 30f;
    [SerializeField] private float bounceDuration = 0.5f;
    [SerializeField] private float delayBetweenStars = 0.2f;

    public static bool isCounting = false;
    #region innit & Lifecycle
    private void Start() {
        this.ResetPauseUI();
        CalculateTime();
    }
    private void OnEnable() {
        GameManager.OnWinGame += OnWinGame;
        GameManager.OnGameOver += OnGameOver;
    }
    private void OnDisable() {
        GameManager.OnWinGame -= OnWinGame;
        GameManager.OnGameOver -= OnGameOver;
    }
    private void Update() {
        if (isCounting) {
            this.CalculateTime();
        }
    }
    #endregion
    private void ResetPauseUI() {
        pauseButton.SetActive(true);
        pauseUI.anchoredPosition = new Vector2(0, -Screen.height);
        pauseUI.gameObject.SetActive(false);
    }
    private void CalculateTime() {
        timerText.text = GameManager.instance.CalTime();
        if(GameManager.currRemainingTime <= 10f) timerText.color = Color.red;
    }

    public void OnRestartPress() {
        AudioManager.instance.PlaySound("buttonClick");
        GameManager.TogglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPausePress() {
        AudioManager.instance.PlaySound("buttonClick");
        pauseButton.SetActive(false);
        pauseUI.gameObject.SetActive(true);
        GameManager.TogglePause();

        pauseUI.DOAnchorPos(Vector3.zero, 1f).SetEase(Ease.OutQuad).SetUpdate(true);

        pauseUI.localScale = Vector3.one * 0.6f;
        pauseUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnResumeGamePress() {
        AudioManager.instance.PlaySound("buttonClick");
        pauseButton.SetActive(true);
        pauseUI.DOAnchorPos(new Vector2(0, -Screen.height), 0.5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true).OnComplete(() => {
                pauseUI.gameObject.SetActive(false);
                GameManager.TogglePause();
            });
    }

    public void OnWinGame() {
        pauseButton.SetActive(false);
        winUI.gameObject.SetActive(true);
        recordText.text = timerText.text;

        winUI.localScale = Vector3.one * 0.6f;
        winUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);

        AnimateStars();

        GameManager.TogglePause();
        LevelHandler.UnlockNextLevel();
    }

    private void AnimateStars() {
        for (int i = 0; i < Stars.Length; i++) {
            RectTransform star = Stars[i];
            float initialY = star.anchoredPosition.y;
            foreach (var obj in Stars) {
                if (obj != null) obj.gameObject.SetActive(true);
            }
            Sequence sequence = DOTween.Sequence();
            sequence.Append(star.DOAnchorPosY(initialY + bounceHeight, bounceDuration).SetEase(Ease.OutQuad)).SetUpdate(true);
            sequence.Append(star.DOAnchorPosY(initialY, bounceDuration).SetEase(Ease.InQuad)).SetUpdate(true);
            sequence.SetDelay(i * delayBetweenStars).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        }
    }

    public void OnGameOver() {
        pauseButton.SetActive(false);
        gameOverUI.gameObject.SetActive(true);

        gameOverUI.localScale = Vector3.one * 0.6f;
        gameOverUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);

        GameManager.TogglePause();
        HeartsSystem.LoseHeart();
    }

    public void OnExitLevelPress() {
        AudioManager.instance.PlaySound("buttonClick");
        DOTween.Kill(pauseUI);
        DOTween.Kill(winUI);
        DOTween.Kill(gameOverUI);
        SceneManager.LoadScene(1);
        GameManager.TogglePause();
    }
}
