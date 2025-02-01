using UnityEngine;
public class Pipe : MonoBehaviour {
    [SerializeField] int type = -1;
    public int GetPipeType() {
        return type;
    }
}
