using UnityEngine;

public class Donut : MonoBehaviour {
    public static Donut instance;
    private Rigidbody2D rb;
    private void Update() {
        rb.AddForce(rb.velocity.normalized * 0.5f, ForceMode2D.Force);
    }
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        if(instance==null) instance = this;
    }
    public void SetDonutGravity(float _scale) {
        rb.gravityScale =  _scale;
    }
}
