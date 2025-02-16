using UnityEngine;

public class TriggerOutScreenLose : MonoBehaviour {
    [SerializeField] GameObject donut;
    private void Awake() {
        if(donut == null) {
            donut = GameObject.Find(GameManager.instance.donutName);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform == donut.transform) {
            GameManager.OnGameOverInvoke();
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
