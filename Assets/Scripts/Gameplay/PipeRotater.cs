using System.Collections;
using UnityEngine;

public class PipeRotater : MonoBehaviour {
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotateAngle = 90f;
    private float targetAngle;
    private bool isRotating;
    private bool isPaused;

    private void OnEnable() {
        GameManager.OnPauseStateChanged += HandlePauseStateChanged;
    }
    private void OnDisable() {
        GameManager.OnPauseStateChanged -= HandlePauseStateChanged;
    }
    private void Start() {
        targetAngle = transform.eulerAngles.z;
    }
    private void OnMouseDown() {
        if (!isPaused && !isRotating) {
            targetAngle = (transform.eulerAngles.z + rotateAngle) % 360;
            StartCoroutine(RotatePipe());
        }
    }
    private void HandlePauseStateChanged(bool paused) => isPaused = paused;

    private IEnumerator RotatePipe() {
        isRotating = true;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.1f) {
            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime));
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        isRotating = false;
    }
}