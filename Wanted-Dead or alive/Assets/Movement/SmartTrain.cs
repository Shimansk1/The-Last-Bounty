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

    // Interní promìnné
    private Transform[] waypoints;
    private int currentPointIndex;

    // Pro zastávky potøebujeme vìdìt, jestli je hráè uvnitø
    private bool playerOnBoard = false;
    private CharacterController playerCC; // Jen pro vyhazování na zastávce

    // --- PRO VAGÓNY (Historie) ---
    public List<Vector3> breadcrumbs = new List<Vector3>();

    void Start()
    {
        // Naètení bodù
        if (trackParent != null)
        {
            int childCount = trackParent.childCount;
            waypoints = new Transform[childCount];
            for (int i = 0; i < childCount; i++) waypoints[i] = trackParent.GetChild(i);
        }

        // Startovní pozice
        currentPointIndex = startPointIndex;
        if (waypoints.Length > 0) transform.position = waypoints[currentPointIndex].position;
    }

    void Update()
    {
        // 1. Nahrávání historie pro vagóny
        if (breadcrumbs.Count == 0 || Vector3.Distance(transform.position, breadcrumbs[breadcrumbs.Count - 1]) > 0.1f)
        {
            breadcrumbs.Add(transform.position);
            if (breadcrumbs.Count > 1500) breadcrumbs.RemoveAt(0);
        }

        // --- POKUD VLAK STOJÍ NEBO NEMÁ TRAT, DÁL NEJDEME ---
        if (isStopped || waypoints.Length == 0) return;

        // 2. Jízda Vlaku
        Transform targetPoint = waypoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Otáèení
        Vector3 direction = targetPoint.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        // 3. Dojeli jsme k bodu?
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

        // Vyhození hráèe na zastávce
        if (playerOnBoard && station.exitPoint != null)
        {
            ForceDisembark(station.exitPoint);
        }

        yield return new WaitForSeconds(station.waitTime);

        isStopped = false;
        GoToNextPoint();
    }

    // --- JENOM PARENTING (Žádné složité výpoèty) ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBoard = true;
            playerCC = other.GetComponent<CharacterController>();
            // Tady se stane to kouzlo - hráè se stane dítìtem vlaku
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBoard = false;
            playerCC = null;
            // Odlepení
            other.transform.SetParent(null);
        }
    }

    void ForceDisembark(Transform exitLocation)
    {
        if (playerCC != null)
        {
            playerCC.enabled = false; // Vypneme fyziku
            playerCC.transform.SetParent(null); // Odlepíme
            playerCC.transform.position = exitLocation.position; // Teleport
            playerCC.transform.rotation = exitLocation.rotation;
            playerCC.enabled = true; // Zapneme fyziku

            playerOnBoard = false;
            playerCC = null;
        }
    }
}