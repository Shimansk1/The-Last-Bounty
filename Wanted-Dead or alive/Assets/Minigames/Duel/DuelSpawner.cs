using UnityEngine;

[System.Serializable]
public struct CitySpawnLocation
{
    public Transform spawnPoint;
    public CityName city;
}

public class DuelSpawner : MonoBehaviour
{
    [Header("Nastaveni Spawnu")]
    public GameObject duelistPrefab;

    [Tooltip("Vypln body spawnu a vyber k nim i spravne mesto")]
    public CitySpawnLocation[] spawnLocations;

    public float spawnInterval = 900f;

    private float timer;
    private GameObject activeDuelist;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnDuelist();
            timer = spawnInterval;
        }
    }

    public void SpawnDuelist()
    {
        if (duelistPrefab == null || spawnLocations.Length == 0) return;

        if (activeDuelist != null)
        {
            Destroy(activeDuelist);
        }

        int randomIndex = Random.Range(0, spawnLocations.Length);
        CitySpawnLocation selectedLocation = spawnLocations[randomIndex];

        activeDuelist = Instantiate(duelistPrefab, selectedLocation.spawnPoint.position, selectedLocation.spawnPoint.rotation);

        NPCController npcScript = activeDuelist.GetComponent<NPCController>();
        if (npcScript != null)
        {
            npcScript.currentCity = selectedLocation.city;
        }

        Debug.Log($"Novy duelista se objevil ve meste {selectedLocation.city} na bode {selectedLocation.spawnPoint.name}");
    }
}