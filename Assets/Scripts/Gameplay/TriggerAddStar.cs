using UnityEngine;

public class TriggerAddStar : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        StarsSystems.stars++;
        GameManager.OnAddStarInvoke(gameObject);
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
