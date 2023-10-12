using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRubber : MonoBehaviour
{
    [SerializeField] private Transform waypointsContainer;
    private List<Transform> waypoints;
    [SerializeField] private float speed;

    private int nextWaypointIndex;

    private Transform previousWaypoint;
    private Transform nextWaypoint;

    private float timeMove;
    private float elapsedTime;
    void Start()
    {
        waypoints = new List<Transform>();
        foreach (Transform waypoint in waypointsContainer)
        {
            waypoints.Add(waypoint);
        
        }
        SetNextWaypoint();
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeMove;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, nextWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        previousWaypoint = GetWaypoint(nextWaypointIndex);
        nextWaypointIndex = GetNextWaypointIndex(nextWaypointIndex);
        nextWaypoint = GetWaypoint(nextWaypointIndex);

        elapsedTime = 0;

        float distance = Vector3.Distance(previousWaypoint.position, nextWaypoint.position);
        timeMove = distance / speed;
    }
    public Transform GetWaypoint(int waypointIndex)
    {
        return waypoints[waypointIndex];
    }
    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        return nextWaypointIndex;
    }
}
