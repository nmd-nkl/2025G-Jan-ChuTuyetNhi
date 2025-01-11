using UnityEngine;
public class PipeStatus : MonoBehaviour {
    [SerializeField] int type = -1;
    private Vector3 originalRotate;
    private bool prevStatus = true;

    private void OnEnable() {
        InitPipeStatus();
    }
    private void InitPipeStatus() {
        originalRotate = transform.rotation.eulerAngles;
        prevStatus = true;
    }
    public bool IsCorrectPlaced() {
        int currentAngle = (int) Mathf.Repeat(transform.rotation.eulerAngles.z, 360f);
        int correctAngle = (int) Mathf.Repeat(originalRotate.z, 360f);
        if (type != 0) {
            return currentAngle == correctAngle;
        } else {//Case for I-Pipe (type==0)
            return currentAngle % 180 == correctAngle % 180;
        }
    }

    public void UpdateStatus() {
        bool currStatus = IsCorrectPlaced();
        if (currStatus != prevStatus) {
            int status = currStatus ? 1 : -1;
            prevStatus = currStatus;
            GameManager.OnPipeStatusChangedInvoke(status);
        }
    }
    public void UpdatePrevStatus() {//Update when the Game enters Lv
        prevStatus = IsCorrectPlaced();
    }
    public int GetPipeType() {
        return type;
    }
}
