using UnityEngine;

public class ClickHandler : MonoBehaviour {
    [SerializeField] bool isTurnOn = true;
    private void OnMouseDown() {
        if(GameManager.isPaused) return; 
        this.HandleForceStatus();
    }
    private void HandleForceStatus() {
        GameObject range = transform.parent.GetChild(1).gameObject;
        if (isTurnOn) {
            range.SetActive(false);
            isTurnOn = false;
        } else {
            range.SetActive(true);
            isTurnOn = true;
        }
    } 
}
