using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildGrid : MonoBehaviour {
    public float cellSize;
    public Vector2 size;
    public GameObject[,] cells;

    public void BuildCells() {
        cells = new GameObject[(int)size.x, (int)size.y];
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                Vector3 pos = CalculatePos(x, y);
                GameObject cell = new GameObject();
                cell.transform.parent = transform;
                cell.transform.position = pos;
                cell.name = $"Cell(x:{x}, y:{y})";

                Cell cellScript = cell.AddComponent<Cell>();
                cellScript.buildArea = this;

                cells[x, y] = cell;
                UnityEditor.EditorUtility.SetDirty(cell);
            }
        }
    }
    private void OnDrawGizmos() {
        //Draw outline cubes
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                Vector3 pos = CalculatePos(x, y);
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, cellSize, 0));
            }
        }
    }
    private Vector3 CalculatePos(int x, int y) {
        return new Vector3(transform.position.x + (x * cellSize), transform.position.y + (y * cellSize), 0);
    }
    public string[,] GridDataToStringMatrix() {
        string[,] matrix = new string[(int)size.x, (int)size.y];
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                GameObject cell = cells[x, y];
                PipeStatus pipe = cell.GetComponentInChildren<PipeStatus>();
                if(pipe==null) {
                    matrix[x, y] = "-1";
                    continue;
                }
                matrix[x, y] = pipe.GetPipeType().ToString() + ":" + cell.transform.GetChild(0).rotation.eulerAngles.z.ToString();
            }
        }
        return matrix;
    } 
}
