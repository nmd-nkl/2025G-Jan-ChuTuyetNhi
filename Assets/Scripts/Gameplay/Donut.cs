using System.Collections;
using UnityEngine;

public class Donut : MonoBehaviour {
    public static Donut instance;
    private Rigidbody2D rb;
    [SerializeField] float _time;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        if(instance==null) instance = this;
    }
    public void SetDonutGravity(float _scale) {
        rb.gravityScale =  _scale;
    }
}
