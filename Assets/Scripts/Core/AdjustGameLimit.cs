using UnityEngine;

public class AdjustGameLimit : MonoBehaviour {
    private PolygonCollider2D polygonCollider;
    public float GAP = 3f;
    public static Vector3 startDolly = Vector2.zero;
    public static Vector3 endDolly = Vector2.zero;
    void Start() {
        polygonCollider = GetComponent<PolygonCollider2D>();
        AdjustCollider();
    }

    void AdjustCollider() {
        Transform parent = GameObject.FindWithTag("Level").transform;
        var allRenderers = parent.GetComponentsInChildren<Renderer>();

        Bounds bounds = allRenderers[0].bounds;
        foreach (var rend in allRenderers) {
            bounds.Encapsulate(rend.bounds);
        }
        float gapLeft = GAP;
        float gapRight = GAP * 2;
        float gapY = GAP;

        Vector2[] points = new Vector2[]
        {
            new Vector2(bounds.min.x - gapLeft, bounds.min.y - gapY), // Bottom-left
            new Vector2(bounds.min.x - gapLeft, bounds.max.y + gapY), // Top-left
            new Vector2(bounds.max.x + gapRight, bounds.max.y + gapY), // Top-right (Mở rộng hơn)
            new Vector2(bounds.max.x + gapRight, bounds.min.y - gapY), // Bottom-right (Mở rộng hơn)
        };

        startDolly = new Vector3(bounds.min.x+10f, bounds.max.y, -10f);
        endDolly = new Vector3(bounds.max.x, bounds.min.y+3f, -10f);
        AdjustDollyTrack.UpdateDolly();
        polygonCollider.SetPath(0, points);
    }
}
