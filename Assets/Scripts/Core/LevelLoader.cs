using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public Transform levelParent;

    private void Start() {
        StarsSystems.stars = 0;
        int selectedLevel = LevelManager.CurrLevel;
        string prefabName = "LevelPrefabs/Level-" + selectedLevel;
        GameObject levelPrefab = Resources.Load<GameObject>(prefabName);
        if (levelPrefab != null) {
            Instantiate(levelPrefab, levelParent);
        }
    }
}