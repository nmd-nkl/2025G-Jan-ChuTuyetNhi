using UnityEngine;

public class ResultManager : MonoBehaviour {
    private int currIncorrectPipeCnt = 0;
    private void OnEnable() {
        PipeEvents.RegisterStatusChanged(HandlePipeStatusChanged);
        this.InitCount();
    }
    private void OnDisable() {
        PipeEvents.UnregisterStatusChanged(HandlePipeStatusChanged);
    }
    private void InitCount() {
        currIncorrectPipeCnt = 0;
    }
    private void HandlePipeStatusChanged(int status) {
        currIncorrectPipeCnt += status;
        Debug.Log("Current Correct Pipes: " + currIncorrectPipeCnt);
        if (currIncorrectPipeCnt == 0) {
            Debug.Log("Win!");
        }
    }
}
