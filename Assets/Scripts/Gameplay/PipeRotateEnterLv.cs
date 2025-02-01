using UnityEngine;

public class PipeRotateEnterLv : MonoBehaviour {
    private void Start() {
        float[] newRotate = { 90, 180, 270 };
        transform.Rotate(new Vector3(0, 0, newRotate[Random.Range(0, newRotate.Length)]));;
    }
}
