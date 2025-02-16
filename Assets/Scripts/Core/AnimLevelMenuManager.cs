using DG.Tweening;
using UnityEngine;

public class AnimLevelMenuManager : MonoBehaviour {
    [SerializeField] Transform monster;
    [SerializeField] Transform candy;
    [SerializeField] Transform cloud;
    [SerializeField] Transform cloud2;
    private void Start() {
        MonsterDancing(monster);
        CandyDancing(candy);
        MonsterDancing(cloud);
        MonsterDancing(cloud2);
    }
    private void OnDisable() {
        DOTween.KillAll();
    }
    private void MonsterDancing(Transform obj) {
        obj.transform.DOScaleY(1.15f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }
    private void CandyDancing(Transform obj) {
        obj.transform.DORotate(new Vector3(0, 0, 10), 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }

}
