using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Cell : MonoBehaviour {
    public BuildGrid buildArea;
    private void OnDrawGizmos() {
        if (buildArea == null) return;
        Gizmos.color = new Color(0, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.position, 
            new Vector3(buildArea.cellSize * 0.75f, buildArea.cellSize * 0.75f, 0));
    }
}
