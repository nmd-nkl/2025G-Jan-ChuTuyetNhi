using UnityEngine;

public class TriggerAtTargetWin : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
            GameManager.OnWinGameInvoke();
        }
    }
}
