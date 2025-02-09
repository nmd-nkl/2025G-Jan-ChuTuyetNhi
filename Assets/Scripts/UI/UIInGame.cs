using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;

public class UIInGame : MonoBehaviour {
    #region UI Elements
    [Header("UI")]
    [SerializeField] private RectTransform settingsUI;
    [SerializeField] private RectTransform pauseButton;
    [SerializeField] private RectTransform pauseUI;
    [SerializeField] private RectTransform winUI;
    [SerializeField] private RectTransform gameOverUI;
    [SerializeField] private RectTransform starIconUI;
    [SerializeField] private RectTransform[] Stars;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI StarCountTxt;
    #endregion

    #region Animation Settings
    [Header("Anim Win Values")]
    public float bounceHeight = 30f;
    public float bounceDuration = 0.5f;
    public float delayBetweenStars = 0.2f;

    public float scaleDuration = 0.2f;
    public float moveDuration = 0.5f;
    public float starbounceHeight = 10f;
    #endregion

    #region Game State
    public static bool isCounting = false;
    private bool IsClockPlaying = false;
    #endregion

    #region Lifecycle Methods
    private void Start() {
        GameManager.ResetGameOverState();
        GameManager.instance.ResetCountingTime();
        ResetPauseUI();
        CalculateTime();
        isCounting = false;
        IsClockPlaying = false;
    }

    private void OnEnable() {
        GameManager.OnWinGame += OnWinGame;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnAddStar += HandleStarFlying;
    }

    private void OnDisable() {
        GameManager.OnWinGame -= OnWinGame;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnAddStar -= HandleStarFlying;
    }

    private void Update() {
        if (isCounting) {
            CalculateTime();
        }
    }
    #endregion

    #region UI Handling
    private void ResetPauseUI() {
        pauseButton.gameObject.SetActive(true);
        pauseUI.anchoredPosition = new Vector2(0, -Screen.height);
        pauseUI.gameObject.SetActive(false);
    }

    private void CalculateTime() {
        timerText.text = GameManager.instance.CalTime();
        if (GameManager.currRemainingTime <= 10f) {
            timerText.color = Color.red;
            if (!IsClockPlaying) {
                AudioManager.instance.PauseAll();
                AudioManager.instance.Play(SoundEffect.ClockTimeUp);
                IsClockPlaying = true;
            }
        }
    }
    #endregion

    #region Button Actions
    public void OnRestartPress() {
        AudioManager.instance.Play(SoundEffect.BgMusic);
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        GameManager.TogglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnPausePress() {
        AudioManager.instance.Pause(SoundEffect.ClockTimeUp);
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        pauseButton.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(true);
        GameManager.TogglePause();

        AudioManager.instance.Play(SoundEffect.PopupOpen);
        pauseUI.DOAnchorPos(Vector3.zero, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
        pauseUI.localScale = Vector3.one * 0.6f;
        pauseUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    public void OnResumeGamePress() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        pauseButton.gameObject.SetActive(true);
        pauseUI.DOAnchorPos(new Vector2(0, -Screen.height), 0.5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true).OnComplete(() => {
                pauseUI.gameObject.SetActive(false);
                GameManager.TogglePause();
                AudioManager.instance.ResumeAll();
            });
    }
    public void OnExitLevelPress() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        DOTween.KillAll();
        SceneManager.LoadScene(1);
        GameManager.TogglePause();
        AudioManager.instance.Play(SoundEffect.BgMusic);
    }
    public void OnSettingsPress() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);

        pauseUI.gameObject.SetActive(false);
        settingsUI.gameObject.SetActive(true);
        settingsUI.localScale = Vector3.one * 0.4f;
        settingsUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    public void OnExitSettingsPress() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);

        settingsUI.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad).SetUpdate(true).
            OnComplete(() => {
                settingsUI.gameObject.SetActive(false);
                pauseUI.gameObject.SetActive(true);
            });
    }
    #endregion

    #region Game Events
    public void OnWinGame() {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play(SoundEffect.WinMusic);

        pauseButton.gameObject.SetActive(false);
        winUI.gameObject.SetActive(true);
        recordText.text = timerText.text;
        
        winUI.localScale = Vector3.one * 0.6f;
        winUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
        AnimateStars();

        GameManager.TogglePause();
        LevelHandler.UnlockNextLevel();
        StarsSystems.instance.SaveStarsData();
    }

    public void OnGameOver() {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play(SoundEffect.LoseMusic);

        pauseButton.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
        
        gameOverUI.localScale = Vector3.one * 0.6f;
        gameOverUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);

        GameManager.TogglePause();
        HeartsSystem.instance.LoseHeart();
        StarsSystems.instance.SaveStars(LevelHandler.CurrLevel, 0);
    }
    #endregion

    #region Animations
    private void AnimateStars() {
        for (int i = 0; i < Stars.Length; i++) {
            RectTransform star = Stars[i];
            if (star == null) continue;

            float initialY = star.anchoredPosition.y;
            star.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(star.DOAnchorPosY(initialY + bounceHeight, bounceDuration).SetEase(Ease.OutQuad)).SetUpdate(true);
            sequence.Append(star.DOAnchorPosY(initialY, bounceDuration).SetEase(Ease.InQuad)).SetUpdate(true);
            sequence.SetDelay(i * delayBetweenStars).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        }
    }

    public void HandleStarsCountUI() {
        StarCountTxt.text = StarsSystems.stars + "/3";
        Sequence starSequence = DOTween.Sequence();
        starSequence.Append(starIconUI.transform.DOScale(0.8f, scaleDuration * 0.5f))
                    .Append(starIconUI.transform.DOScale(1.2f, scaleDuration))
                    .Append(starIconUI.transform.DOScale(1f, scaleDuration * 0.5f));
    }

    public void HandleStarFlying(GameObject star) {
        Vector3 uiPosition = starIconUI.position;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(uiPosition);
        worldPosition.z = 0;
        Sequence starSequence = DOTween.Sequence();
        starSequence.Append(star.transform.DOScale(1.2f, scaleDuration))
                    .Append(star.transform.DOScale(0.8f, scaleDuration))
                    .Append(star.transform.DOScale(1f, scaleDuration * 0.5f))
                    .Append(star.transform.DOMove(worldPosition, moveDuration).SetEase(Ease.InQuad))
                    .OnComplete(() => {
                        Destroy(star);
                        HandleStarsCountUI();
                    });
    }
    #endregion
    #region Audio Settings
    public void SetMasterVolume(float volume) => AudioManager.instance.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, volume));
    public void SetMusicVolume(float volume) => AudioManager.instance.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80f, 0f, volume));
    public void SetSFXVolume(float volume) => AudioManager.instance.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80f, 0f, volume));
    #endregion
}
