using Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraInGameManager : MonoBehaviour {
    public static CameraInGameManager instance;
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    private void Awake() {
        if (instance == null) instance = this;
    }
    private void Start() {
        if (_allVirtualCameras == null || _allVirtualCameras.Length == 0) {
            _allVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        }
        this.SwitchCamera(0);
    }
    public void SwitchCamera(int cameraIdx) {
        foreach (var cam in _allVirtualCameras) {
            cam.gameObject.SetActive(false);
        }
        _allVirtualCameras[cameraIdx].gameObject.SetActive(true);
    }
    public void HandleFollowDonutCamera(CinemachineVirtualCamera followDonutCam) {
        StartCoroutine(WaitCamCoroutine(1.5f));
        Donut.instance.SetDonutGravity(1);
        UIInGame.isCounting = true;
    }
    private IEnumerator WaitCamCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
    }
}
