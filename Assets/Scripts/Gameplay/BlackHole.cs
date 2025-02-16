using UnityEngine;

public class BlackHole : MonoBehaviour {
    Rigidbody2D donut;
    Transform blackHoleCenter;

    public bool isTurnOn = true;
    public float pullIntensity = 10f;

    private void Awake() {
        if (donut == null) {
            GameObject donutObj = GameObject.Find(GameManager.instance.donutName);
            if (donutObj != null) 
                donut = donutObj.GetComponent<Rigidbody2D>();
        }
        if (blackHoleCenter == null && transform.parent != null) {
                blackHoleCenter = transform.parent.GetChild(0);
        }
    }
    [SerializeField] private float f_OfSin = 2f; // Tần số
    [SerializeField] private float A_OfSin = 1f; // Biên độ

    private void OnTriggerStay2D(Collider2D other) {
        donut.velocity = Vector2.zero;

        if (isTurnOn && other.attachedRigidbody == donut) {
            Vector2 direction = ((Vector2)blackHoleCenter.position - donut.position).normalized;
            donut.AddForce(direction * pullIntensity, ForceMode2D.Force);
            float sinOffset = Mathf.Sin(Time.time * f_OfSin) * A_OfSin;
            donut.AddForce(new Vector2(0, sinOffset), ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        donut.gravityScale = 1f;
        donut.velocity = Vector2.zero;
    }
}
