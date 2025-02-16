using UnityEngine;

public class ClickHandler : MonoBehaviour {
    [SerializeField] private bool isTurnOn = true;
    [SerializeField] private Sprite turnOffHole;
    [SerializeField] private Sprite turnOnHole;
    private GameObject range;
    private SpriteRenderer cell;

    private void Start() {
        range = transform.parent.GetChild(1).gameObject;
        cell = gameObject.GetComponent<SpriteRenderer>();

        isTurnOn = range.activeSelf;
        cell.sprite = isTurnOn ? turnOnHole : turnOffHole;
    }

    private void OnMouseDown() {
        if (GameManager.isPaused) return;
        HandleForceStatus();
    }

    public void HandleForceStatus() {
        AudioManager.instance.Play(SoundEffect.ClickPop);

        if (isTurnOn) {
            cell.sprite = turnOffHole;
            range.SetActive(false);
        } else {
            cell.sprite = turnOnHole;
            range.SetActive(true);
        }

        isTurnOn = !isTurnOn;
    }
}
