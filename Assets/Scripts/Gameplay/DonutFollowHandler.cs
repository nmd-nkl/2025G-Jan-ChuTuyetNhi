using Cinemachine;
using UnityEngine;

public class DonutFollowHandler : MonoBehaviour {
    private void OnEnable() {
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if(cinemachineVirtualCamera != null ) 
            CameraInGameManager.instance.HandleFollowDonutCamera(cinemachineVirtualCamera);
    }
}
