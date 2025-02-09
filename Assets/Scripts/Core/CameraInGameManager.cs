using Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraInGameManager : MonoBehaviour {
    public static CameraInGameManager instance;

    [Header("General Settings")]
    [SerializeField] private float waitToStartDonut = 1f;
    public CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Dolly Camera Settings (Review Map)")]
    [SerializeField] private int dollyCameraIndex = 0;
    [SerializeField] private float initialDollyPathPosition = 0f;
    [SerializeField] private float dollySpeed = 1f;

    [Header("Follow Donut Camera Settings")]
    [SerializeField] private int followDonutCameraIndex = 1;
    [SerializeField] private int activePriority = 100;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        HandleReviewMap();
    }
    /// <summary>
    /// Chuyển đổi giữa các virtual camera dựa vào index.
    /// Nếu camera có CinemachineTrackedDolly (như camera dolly track) thì reset vị trí trên track.
    /// </summary>
    public void SwitchCamera(int cameraIdx) {
        foreach (var cam in _allVirtualCameras) {
            cam.Priority = 0;
        }

        CinemachineVirtualCamera selectedCamera = _allVirtualCameras[cameraIdx];
        selectedCamera.Priority = activePriority;

        CinemachineTrackedDolly trackedDolly = selectedCamera.GetComponent<CinemachineTrackedDolly>();
        if (trackedDolly != null) {
            trackedDolly.m_PathPosition = initialDollyPathPosition;
        }
    }

    /// <summary>
    /// Xử lý chuyển sang chế độ review map (camera dolly track).
    /// Khi được gọi, camera dolly sẽ tự động di chuyển từ điểm đầu đến điểm cuối của track.
    /// </summary>
    public void HandleReviewMap() {
        SwitchCamera(dollyCameraIndex);
        CinemachineVirtualCamera dollyCam = _allVirtualCameras[dollyCameraIndex];
        CinemachineTrackedDolly trackedDolly = dollyCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        StartCoroutine(MoveCameraAlongDolly(trackedDolly));
    }

    private IEnumerator MoveCameraAlongDolly(CinemachineTrackedDolly trackedDolly) {
        float maxPathPosition = trackedDolly.m_Path.MaxPos;
        while (trackedDolly.m_PathPosition < maxPathPosition) {
            trackedDolly.m_PathPosition += dollySpeed * Time.deltaTime;
            yield return null;
        }
        trackedDolly.m_PathPosition = maxPathPosition;
        HandleFollowDonutCamera();
        yield break;
    }

    /// <summary>
    /// Chuyển về chế độ follow Donut.
    /// </summary>
    public void HandleFollowDonutCamera() {
        SwitchCamera(followDonutCameraIndex);
        StartCoroutine(HandleCameraTransition());
    }
    private IEnumerator HandleCameraTransition() {
        yield return new WaitUntil(() => !cinemachineBrain.IsBlending);
        yield return new WaitForSeconds(waitToStartDonut);
        
        Donut.instance.SetDonutGravity(1);
        UIInGame.isCounting = true;
    }
}
