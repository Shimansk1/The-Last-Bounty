using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Nastavení itemù")]
    public GameObject[] itemPrefabs; 
    public float spawnInterval = 60f; 
    public int maxSpawnCount = 10;

    [Header("Spawn body")]
    public Transform[] spawnPoints;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Debug.Log("spawnuji-update");
            SpawnItems();
            timer = spawnInterval;
        }
    }

    void SpawnItems()
    {
        foreach (Transform point in spawnPoints)
        {
            if (Random.value < 0.5f) // 50 % šance že na bodu nìco bude
            {
                if (GameObject.FindGameObjectsWithTag("Pickup").Length < 5)
                {
                    GameObject itemToSpawn = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                    Instantiate(itemToSpawn, point.position, Quaternion.identity);
                    Debug.Log("spawnuji");
                }
            }
        }
    }
}
