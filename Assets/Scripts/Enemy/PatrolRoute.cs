using UnityEngine;

[AddComponentMenu("Game/Enemy/Patrol Route")]
public class PatrolRoute : MonoBehaviour
{
    private const float WaypointRadius = 0.15f;

    [SerializeField] private Transform[] _waypoints;

    public int WaypointCount => _waypoints != null ? _waypoints.Length : 0;

    public Transform GetWaypoint(int index)
    {
        return _waypoints[index];
    }

    private void OnDrawGizmos()
    {
        if (_waypoints == null || _waypoints.Length == 0)
            return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < _waypoints.Length; i++)
        {
            Transform waypoint = _waypoints[i];

            if (waypoint == null)
                continue;

            Gizmos.DrawWireSphere(waypoint.position, WaypointRadius);

            if (i + 1 >= _waypoints.Length)
                continue;

            Transform nextWaypoint = _waypoints[i + 1];

            if (nextWaypoint == null)
                continue;

            Gizmos.DrawLine(waypoint.position, nextWaypoint.position);
        }
    }
}