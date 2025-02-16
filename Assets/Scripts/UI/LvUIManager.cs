using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvUIManager : MonoBehaviour {
    [Header("Not Enough Heart")]
    [SerializeField] RectTransform WarnZeroHeart;
    [SerializeField] TextMeshProUGUI healTimeInfoTxt;
    [SerializeField] GameObject WhiteBG;
    [Header("Pages UI")]
    [SerializeField] RectTransform nextPage;
    [SerializeField] RectTransform prevPage;
    [Header("Settings")]
    [SerializeField] GameObject settingsButton;
    [SerializeField] RectTransform settingsUI;
    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    #region Singleton
    public static LvUIManager instance;
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
        BounceObj(nextPage);
        BounceObj(prevPage);
        if(HeartsSystem.hearts==0)
            WarnZeroInfoEnable();
    }
    private void OnDisable() {
        DOTween.KillAll();
    }
    public void WarnZeroInfoEnable() {
        settingsButton.SetActive(false);
        WhiteBG.gameObject.SetActive(true);
        WarnZeroHeart.gameObject.SetActive(true);
        WarnZeroHeart.localScale = Vector3.one * 0.4f;
        WarnZeroHeart.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    private void Update() {
        healTimeInfoTxt.text = HeartsSystem.instance.GetHealTimeTxt();
    }
    public void OnExitWarnZeroHeart() {
        settingsButton.SetActive(true);
        WhiteBG.gameObject.SetActive(false);
        WarnZeroHeart.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad).SetUpdate(true).
            OnComplete(() => {
                WarnZeroHeart.gameObject.SetActive(false);
            });
    }
    public void AnimHeart(RectTransform HeartIcon) {

        float scaleDuration = 0.2f;
        float moveDuration = 0.5f;
        float starbounceHeight = 10f;

        Sequence heartSequence = DOTween.Sequence();
        heartSequence.Append(HeartIcon.transform.DOScale(0.8f, scaleDuration * 0.5f))
                    .Append(HeartIcon.transform.DOScale(1.2f, scaleDuration))
                    .Append(HeartIcon.transform.DOScale(1f, scaleDuration * 0.5f))
                    .Join(HeartIcon.transform.DOLocalMoveY(starbounceHeight, moveDuration * 0.3f).SetRelative().SetEase(Ease.OutQuad))
                    .Append(HeartIcon.transform.DOLocalMoveY(-starbounceHeight, moveDuration * 0.3f).SetRelative().SetEase(Ease.InQuad));
    }
    public void BounceObj(RectTransform uiObj) {
        float bounceHeight = 10f;
        float duration = 0.4f;
        uiObj.DOAnchorPosY(uiObj.anchoredPosition.y + bounceHeight, duration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void SetUpAllSliders() {
        AudioManager.instance.SetUpSlider(masterSlider, "MasterVolume");
        AudioManager.instance.SetUpSlider(musicSlider, "MusicVolume");
        AudioManager.instance.SetUpSlider(sfxSlider, "SFXVolume");
    }
    public void OnSettingsPress() {
        this.SetUpAllSliders();

        WhiteBG.SetActive(true);
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        settingsButton.SetActive(false);
        settingsUI.gameObject.SetActive(true);

        settingsUI.localScale = Vector3.one * 0.9f;
        settingsUI.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    public void OnSettingsXPress() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        WhiteBG.SetActive(false);

        settingsUI.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() => {
            settingsUI.gameObject.SetActive(false);
            settingsButton.SetActive(true);
        });
    }
    #region HandleSilders
    public void SetMasterVolume(float volume) => AudioManager.instance.SetMasterVolume(volume);
    public void SetMusicVolume(float volume) => AudioManager.instance.SetMusicVolume(volume);
    public void SetSFXVolume(float volume) => AudioManager.instance.SetSFXVolume(volume);
    #endregion
}
