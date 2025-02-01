using Cinemachine;
using UnityEngine;
using System.Collections;

public class PreviewLvCam : MonoBehaviour {
    [SerializeField] float waitToStartTime = 1.0f;
    [SerializeField] float goDownSpeed = 1.0f;
    [SerializeField] float goUpSpeed = 1.0f;
    [SerializeField] private Collider2D cameraBounds;

    private float targetY;
    private bool isMoving = false;
    private float speed;
    private Vector3 initialPosition;

    private void Start() {
        CameraInGameManager.instance.SwitchCamera(0);
        UIInGame.isCounting = false;
        Donut.instance.SetDonutGravity(0);
        StartCoroutine(WaitCamCoroutine(waitToStartTime));
        initialPosition = new Vector3 (0, cameraBounds.bounds.max.y, 0);
        GoDown();
    }

    private IEnumerator WaitCamCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
    }

    private void Update() {
        if (isMoving) MoveCamera();
    }

    private void MoveCamera() {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(currentPosition.x, targetY, currentPosition.z);
        Vector3 direction = (targetPosition - currentPosition).normalized;

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.y - targetY) < 0.01f) {
            isMoving = false;
            transform.position = targetPosition;

            if (Mathf.Approximately(targetY, cameraBounds.bounds.min.y)) {
                GoUp();
            } else if (Mathf.Approximately(targetY, cameraBounds.bounds.max.y)) {
                targetY = initialPosition.y;
                StartCoroutine(WaitCamCoroutine(1f));
                CameraInGameManager.instance.SwitchCamera(1);
            }
        }
    }
    private void GoDown() {
        targetY = cameraBounds.bounds.min.y;
        speed = goDownSpeed;
        isMoving = true;
    }
    private void GoUp() {
        targetY = cameraBounds.bounds.max.y;
        speed = goUpSpeed;
        isMoving = true;
    } 
}
