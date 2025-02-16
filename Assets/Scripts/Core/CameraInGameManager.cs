using Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraInGameManager : MonoBehaviour {
    public static CameraInGameManager instance;

    [Header("General Settings")]
    public float waitToStartDonut = 1f;
    public CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Dolly Camera Settings (Review Map)")]
    [SerializeField] private int dollyCameraIndex = 0;
    [SerializeField] private float initialDollyPathPosition = 0f;
    [SerializeField] private float dollySpeed = 1f;

    [Header("Follow Donut Camera Settings")]
    [SerializeField] private int followDonutCameraIndex = 1;
    [SerializeField] private int activePriority = 100;

    [Header("Drag Camera Settings")]
    [SerializeField] private int DragCameraIdx = 2;
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private float dragLimitX = 15f;
    [SerializeField] private float dragLimitY = 10f;

    private Vector2 lastMousePosition;
    private bool dragPanMoveActive = false;
    private bool dragMode = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        dragPanMoveActive = false;
    }

    private void Start() {
        HandleReviewMap();
    }
    private void Update() {
        if (dragMode) {
            HandleInput();
            if (dragPanMoveActive) {
                HandleDragToPanCamera();
            }
        }
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
        UIInGame.instance.CountingDonut(waitToStartDonut);
        yield return new WaitForSeconds(waitToStartDonut);
        
        Donut.instance.SetDonutGravity(1);
        UIInGame.isCounting = true;
        dragMode = true;
    }

    private void HandleInput() {
        if (!dragMode) return; 
        if (Input.GetMouseButtonDown(0)) {
            if (IsClickingOnPipe()) {
                return; 
            }
            SwitchCamera(DragCameraIdx);
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) {
            SwitchCamera(followDonutCameraIndex);
            dragPanMoveActive = false;
        }
    }
    private void HandleDragToPanCamera() {
        if (!dragMode) return;

        Vector2 mouseMovementDelta = Vector2.zero;

        if (Input.touchCount > 0) { 
            Touch touch = Input.GetTouch(0);
            mouseMovementDelta = touch.deltaPosition;
        } else {
            mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = new Vector3(-mouseMovementDelta.x, -mouseMovementDelta.y, 0);
        Vector3 newPosition = transform.position + moveDir * dragSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -dragLimitX, dragLimitX);
        newPosition.y = Mathf.Clamp(newPosition.y, -dragLimitY, dragLimitY);

        transform.position = newPosition;
    }
    private bool IsClickingOnPipe() {
        Vector2 worldPoint;

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
        } else if (Input.GetMouseButtonDown(0)) {
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else {
            return false;
        }
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject.name == "Pipe";
    }
}
