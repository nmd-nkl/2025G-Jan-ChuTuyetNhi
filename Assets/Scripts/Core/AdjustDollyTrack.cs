using Cinemachine;
using System.Net;
using UnityEngine;

public class AdjustDollyTrack : MonoBehaviour {
    private static CinemachineSmoothPath dollyTrack;
    public void Start() {
        dollyTrack = GetComponent<CinemachineSmoothPath>();
        dollyTrack.m_Waypoints = new CinemachineSmoothPath.Waypoint[2];
        dollyTrack.m_Waypoints[0] = new CinemachineSmoothPath.Waypoint { position = AdjustGameLimit.startDolly };
        dollyTrack.m_Waypoints[1] = new CinemachineSmoothPath.Waypoint { position = AdjustGameLimit.endDolly};
        dollyTrack.InvalidateDistanceCache();
    }
    public static void UpdateDolly() {
        dollyTrack.m_Waypoints[0] = new CinemachineSmoothPath.Waypoint { position = AdjustGameLimit.startDolly };
        dollyTrack.m_Waypoints[1] = new CinemachineSmoothPath.Waypoint { position = AdjustGameLimit.endDolly };
    }
}
