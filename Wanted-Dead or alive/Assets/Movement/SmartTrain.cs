using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartTrain : MonoBehaviour
{
    [Header("Trat a Pohyb")]
    public Transform trackParent;
    public float speed = 10f;
    public float turnSpeed = 5f;
    public int startPointIndex = 0;

    [Header("Logika")]
    public bool isStopped = false;
    public Transform currentExitPoint;

    private Transform[] waypoints;
    private int currentPointIndex;
    public List<Vector3> breadcrumbs = new List<Vector3>();

    void Start()
    {
        if (trackParent != null)
        {
            int childCount = trackParent.childCount;
            waypoints = new Transform[childCount];
            for (int i = 0; i < childCount; i++) waypoints[i] = trackParent.GetChild(i);
        }

        currentPointIndex = startPointIndex;
        if (waypoints.Length > 0) transform.position = waypoints[currentPointIndex].position;
    }

    void Update()
    {
        if (breadcrumbs.Count == 0 || Vector3.Distance(transform.position, breadcrumbs[breadcrumbs.Count - 1]) > 0.1f)
        {
            breadcrumbs.Add(transform.position);
            if (breadcrumbs.Count > 1500) breadcrumbs.RemoveAt(0);
        }

        if (isStopped || waypoints.Length == 0) return;

        Transform targetPoint = waypoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        Vector3 direction = targetPoint.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            StationPoint station = targetPoint.GetComponent<StationPoint>();

            if (station != null)
            {
                StartCoroutine(StopAtStation(station));
            }
            else
            {
                GoToNextPoint();
            }
        }
    }

    void GoToNextPoint()
    {
        currentPointIndex++;
        if (currentPointIndex >= waypoints.Length) currentPointIndex = 0;
    }

    IEnumerator StopAtStation(StationPoint station)
    {
        isStopped = true;
        currentExitPoint = station.exitPoint;

        yield return new WaitForSeconds(station.waitTime);

        currentExitPoint = null;
        isStopped = false;

        GoToNextPoint();
    }
}