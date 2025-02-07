using UnityEngine;
public class PrefabData : MonoBehaviour {
    [SerializeField] int type = -1;
    public int GetPrefabType() {
        return type;
    }
}
