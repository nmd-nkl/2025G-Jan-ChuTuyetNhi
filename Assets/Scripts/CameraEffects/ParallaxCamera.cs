using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour {
    public delegate void ParallaxCameraDelegate(float deltaX, float deltaY);
    public ParallaxCameraDelegate onCameraTranslate;

    private Vector3 oldPosition;
    private CinemachineBrain cinemachineBrain;

    void Start() {
        oldPosition = transform.position;
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    void LateUpdate() {
        if (cinemachineBrain == null) return;

        if (transform.position != oldPosition) {
            float deltaX = oldPosition.x - transform.position.x;
            float deltaY = oldPosition.y - transform.position.y;

            onCameraTranslate?.Invoke(deltaX, deltaY);
            oldPosition = transform.position;
        }
    }
}
