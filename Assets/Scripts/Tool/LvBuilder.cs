using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;

public class LvBuilder : EditorWindow {
    #region Editor Variables
    string[] tabs = new string[] { "Setup", "Pipes" };
    Vector3 originPos;
    string originName = "Level";
    int gridW, gridH;
    float cellSize;
    float pipeSize = 0.39f;
    #endregion
    #region Selected Variables
    int currSelectedIdx = 0;
    int selectedPipeIdx = 0;
    Cell selectedCell;
    bool deleteIsOn = false;
    bool rotateIsOn = false;
    #endregion

    [MenuItem("Level Builder/Open Builder")]
    static void ShowWindow() {
        LvBuilder window = (LvBuilder)GetWindow(typeof(LvBuilder));
        window.Show();
    }
    private void OnGUI() {
        currSelectedIdx = GUILayout.Toolbar(currSelectedIdx, tabs);

        switch (currSelectedIdx) {
            case 0:
                DrawSetupTab();
                break;
            case 1:
                DrawPipesTab("Pipes");
                break;
        }
    }

    private void DrawSetupTab() {
        deleteIsOn = EditorGUILayout.Toggle("Delete Mode", deleteIsOn);
        EditorGUI.BeginDisabledGroup(deleteIsOn);
        rotateIsOn = EditorGUILayout.Toggle("Rotate Mode", rotateIsOn);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(20);
        originPos = EditorGUILayout.Vector3Field("Origin Location", originPos);
        originName = EditorGUILayout.TextField("Origin Name", originName);
        GUILayout.Space(10);

        gridW = EditorGUILayout.IntSlider("Level Width", gridW, 1, 100);
        gridH = EditorGUILayout.IntSlider("Level Height", gridH, 1, 100);
        cellSize = EditorGUILayout.Slider("Cell Size", cellSize, 0, 100);
        pipeSize = EditorGUILayout.Slider("Pipe Size", pipeSize, 0, 100);

        if (GUILayout.Button("Create Grid")) BuildGrid();
        GUILayout.Space(30);

        if (GUILayout.Button("Export Grid to TXT")) {
            GameObject gridObject = GameObject.Find(originName);
            if (gridObject == null) {
                Debug.LogError("Grid not found.");
                return;
            }
            BuildGrid grid = gridObject.GetComponent<BuildGrid>();
            ExportGridToTxt(grid);
        }

        if (GUILayout.Button("Import Grid from TXT")) ImportGridFromTxt();
    }
    private void BuildGrid() {
        GameObject buildGrid = new GameObject();
        buildGrid.transform.position = originPos;
        buildGrid.name = originName;
        BuildGrid area = buildGrid.AddComponent<BuildGrid>();
        area.size = new Vector2(gridW, gridH);
        area.cellSize = this.cellSize;
        area.BuildCells();
        EditorUtility.SetDirty(buildGrid);
    }
    private void DrawPipesTab(string tab) {
        if (!AssetDatabase.IsValidFolder($"Assets/LevelEditor/{tab}")) {
            EditorGUILayout.HelpBox($"Assets/LevelEditor/{tab} missing!", MessageType.Error);
            return;
        }
        GameObject[] prefabs = GetObjects(GetObjectsPath(tab));
        if (prefabs == null || prefabs.Length == 0) {
            EditorGUILayout.HelpBox("No prefabs found in the folder.", MessageType.Info);
            return;
        }
        List<GUIContent> contents = new List<GUIContent>();
        foreach (GameObject prefab in prefabs) {
            if (prefab == null) continue;
            Texture2D previewTexture = GetPreviewTexture(prefab);
            contents.Add(new GUIContent(previewTexture, prefab.name));
        }
        GUIContent[] contentsArray = contents.ToArray();
        selectedPipeIdx = GUILayout.SelectionGrid(selectedPipeIdx, contentsArray, 3);
    }
    private Texture2D GetPreviewTexture(GameObject prefab) {
        return AssetPreview.GetAssetPreview(prefab);
    }
    private string[] GetObjectsPath(string folder) {
        return AssetDatabase.FindAssets("t:GameObject", new string[] { $"Assets/LevelEditor/{folder}" })
                        .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                        .ToArray();
    }
    private GameObject[] GetObjects(string[] paths) {
        if (paths == null || paths.Length == 0) return null;
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (string path in paths) {
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            gameObjects.Add(obj);
        }
        return gameObjects.ToArray();
    }
    #region Process Cell Base On Mode
    private void ClearOldObjects(Cell cell) {
        for (int i = 0; i < cell.transform.childCount; i++) {
            GameObject child = cell.transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }
    }
    private GameObject PlaceNewObject(string folder) {
        GameObject[] prefabs = GetObjects(GetObjectsPath(folder));
        GameObject selectedObject = prefabs[selectedPipeIdx];
        ClearOldObjects(selectedCell);

        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(selectedObject, selectedCell.transform);
        PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        obj.transform.position = selectedCell.transform.position;
        obj.transform.localScale = new Vector2(pipeSize, pipeSize);
        return obj;
    }
    private void RotateObjects(Cell cell, int angle) {
        for (int i = 0; i < cell.transform.childCount; i++) {
            GameObject child = cell.transform.GetChild(i).gameObject;
            child.transform.Rotate(new Vector3(0, 0, angle));
        }
    }
    #endregion
    #region Process Selected Cell
    private void OnSelectionChange() {
        if (Selection.activeGameObject != null) {
            Cell area = Selection.activeGameObject.GetComponent<Cell>();
            if (area != null) {
                selectedCell = area;
                if (deleteIsOn)
                    ClearOldObjects(selectedCell);
                else {
                    if (rotateIsOn)
                        RotateObjects(selectedCell, 90);
                    else
                        PlaceNewObject("Pipes");
                }
            } else {
                selectedCell = null;
            }
        }
    }
    #endregion
    #region File Process
    private void ExportGridToTxt(BuildGrid grid) {
        if (grid == null) {
            Debug.LogError("Grid is null, cannot export.");
            return;
        }

        string path = EditorUtility.SaveFilePanel("Export Grid as TXT", "", $"{originName}_Grid.txt", "txt");
        if (string.IsNullOrEmpty(path)) return;
        string[,] matrix = grid.GridDataToStringMatrix();

        using (StreamWriter writer = new StreamWriter(path)) {
            //Write Grid Data
            writer.Write(originName);
            writer.WriteLine();
            writer.Write(grid.size.x);
            writer.WriteLine();
            writer.Write(grid.size.y);
            writer.WriteLine();
            writer.Write(cellSize);
            writer.WriteLine();
            writer.Write(pipeSize);
            writer.WriteLine();
            //Write Cells data
            for (int y = (int)grid.size.y - 1; y >= 0; y--) {
                for (int x = 0; x < (int)grid.size.x; x++) {
                    writer.Write(matrix[x, y] + ",");
                }
                writer.WriteLine();
            }
        }
        Debug.Log($"Grid exported to {path}");
    }
    private void ImportGridFromTxt() {
        string path = EditorUtility.OpenFilePanel("Import Grid from TXT", "", "txt");
        if (string.IsNullOrEmpty(path)) return;
        string[,] matrix;
        using (StreamReader reader = new StreamReader(path)) {
            //Read Grid data
            originName = reader.ReadLine();
            gridW = int.Parse(reader.ReadLine());
            gridH = int.Parse(reader.ReadLine());
            cellSize = float.Parse(reader.ReadLine());
            pipeSize = float.Parse(reader.ReadLine());
            matrix = new string[gridW, gridH];
            //Read Cells Data
            for (int y = gridH - 1; y >= 0; y--) {
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                for (int x = 0; x < gridW; x++) {
                    matrix[x, y] = values[x];
                }
            }
        }
        GameObject gridObject = GameObject.Find(originName);
        if (gridObject == null) {
            Debug.Log("Grid not found, auto create a new one!");
            this.BuildGrid();
            gridObject = GameObject.Find(originName);
        }
        BuildGrid buildGrid = gridObject.GetComponent<BuildGrid>();
        GameObject[,] cells = buildGrid.cells;
        for(int x = 0; x < gridW; x++) {
            for(int y = 0; y < gridH; y++) {
                if (matrix[x, y] == "-1") continue;
                string[] values = matrix[x, y].Split(":");
                selectedPipeIdx = int.Parse(values[0]);
                selectedCell = cells[x, y].GetComponent<Cell>();
                GameObject obj = PlaceNewObject("Pipes");
                Vector3 currentRotation = obj.transform.rotation.eulerAngles;
                currentRotation.z = int.Parse(values[1]);
                obj.transform.rotation = Quaternion.Euler(currentRotation);
            }
        }
    }
    #endregion
}
