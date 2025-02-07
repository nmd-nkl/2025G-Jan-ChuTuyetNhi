using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {
    public float parallaxFactorX = 0.5f;
    public float parallaxFactorY = 0.3f;

    public void Move(float deltaX, float deltaY) {
        Vector3 newPos = transform.localPosition;
        newPos.x -= deltaX * parallaxFactorX;
        newPos.y -= deltaY * parallaxFactorY;

        transform.localPosition = newPos;
    }
}
