using DG.Tweening;
using TMPro;
using UnityEngine;

public class LvUI : MonoBehaviour {
    [Header("Not Enough Heart")]
    [SerializeField] RectTransform WarnZeroHeart;
    [SerializeField] TextMeshProUGUI healTimeInfoTxt;

    #region Singleton
    public static LvUI instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public void WarnZeroInfoEnable() {
        WarnZeroHeart.gameObject.SetActive(true);
        WarnZeroHeart.localScale = Vector3.one * 0.4f;
        WarnZeroHeart.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    private void Update() {
        healTimeInfoTxt.text = HeartsSystem.instance.GetHealTimeTxt();
    }
    public void OnExitWarnZeroHeart() {
        WarnZeroHeart.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad).SetUpdate(true).
            OnComplete(() => {
                WarnZeroHeart.gameObject.SetActive(false);
            });
    }
}
