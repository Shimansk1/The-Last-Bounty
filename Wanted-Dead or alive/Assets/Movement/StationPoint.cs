using UnityEngine;

public class StationPoint : MonoBehaviour
{
    [Header("Nastavení Zastávky")]
    public float waitTime = 10f;       // Jak dlouho èekat
    public Transform exitPoint;        // SEM pøetáhni prázdný objekt na nástupišti!
    public string stationName = "Nádraží";
}