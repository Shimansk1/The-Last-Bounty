using UnityEngine;

public class StationPoint : MonoBehaviour
{
    [Header("Nastavení Zastávky")]
    public float waitTime = 60f;      // Jak dlouho tu vlak èeká (v sekundách)
    public Transform exitPoint;       // Kam teleportovat hráèe, když se vystupuje
    public string stationName = "Main Town"; // Jen pro info
}