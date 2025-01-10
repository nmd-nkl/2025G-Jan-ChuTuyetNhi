using UnityEngine;
[RequireComponent (typeof(PipeStatus))]
public class PipeRotator : MonoBehaviour {
    PipeStatus pipe;
    private bool isPaused = false;
    private void OnEnable() {
        pipe = GetComponent<PipeStatus>();
        GameManager.OnPauseStateChanged += HandlePauseStateChanged;
    }
    private void OnDisable() {
        GameManager.OnPauseStateChanged -= HandlePauseStateChanged;
    }
    private void Start() {
        this.InitPipeEnterLv();
    }
    private void OnMouseDown() {
        if(isPaused) return;
        this.RotatePipe(90);
    }
    private void InitPipeEnterLv() {
        float[] newRotate = { 90, 180, 270 };
        this.RotatePipe(newRotate[Random.Range(0, newRotate.Length)]);
        pipe.UpdatePrevStatus();
    }
    private void RotatePipe(float angle) {
        transform.Rotate(new Vector3(0, 0, angle));
        pipe.UpdateStatus();
    }
    private void HandlePauseStateChanged(bool paused) {
        isPaused = paused;
    }
}
