using UnityEngine;

public class StepTrigger : MonoBehaviour {
    private void OnMouseDown() {
        if(!TriggerStopForGuide.isStartTutorial) return;
        ClickHandler clickHandler = GetComponent<ClickHandler>();
        PipeRotater pipeRotater = GetComponent<PipeRotater>();
        if (clickHandler != null) TutorialForUser.instance.DoneStep1();
        if (pipeRotater != null) {
            TutorialForUser.instance.DoneStep2();
            TriggerStopForGuide.pauseButton.SetActive(true);
        }
        Destroy(GetComponent<StepTrigger>());
    }
}