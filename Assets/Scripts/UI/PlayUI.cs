using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayUI : MonoBehaviour {
    [SerializeField] Transform playButton;
    private Tween scaleTween;
    public void Start() {
        scaleTween = playButton.DOScale(1.1f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine);
    }
    private void OnDisable() {
        DOTween.KillAll();
    }
    public void OnClickPlayMenu() {
        AudioManager.instance.Play(SoundEffect.ButtonClick);
        SceneManager.LoadScene(1);
    }
}
