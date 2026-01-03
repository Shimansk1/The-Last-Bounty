using UnityEngine;

public class RainSplashManager : MonoBehaviour
{
    public Transform player; // Odkaz na hráèe
    public GameObject splashPrefab; // Particle efekt dopadu
    public float spawnRadius = 5f;
    public int splashCount = 20;
    public LayerMask groundLayer;

    private float spawnInterval = 0.5f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnSplashes();
        }
    }

    void SpawnSplashes()
    {
        for (int i = 0; i < splashCount; i++)
        {
            Vector3 randomPos = player.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                5f, // výška odkud padá raycast dolù
                Random.Range(-spawnRadius, spawnRadius)
            );

            if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 10f, groundLayer))
            {
                GameObject splash = Instantiate(splashPrefab, hit.point, Quaternion.identity);
                Destroy(splash, 2f);
            }
        }
    }
}
