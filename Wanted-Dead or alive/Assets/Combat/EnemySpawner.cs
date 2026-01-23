using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Reference")]
    public Transform player;

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 20f;
    public int maxEnemies = 10;

    private float timer;
    private List<GameObject> spawnedEnemies = new();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawnEnemy();
            timer = 0f;
        }

        spawnedEnemies.RemoveAll(e => e == null);
    }

    void TrySpawnEnemy()
    {
        if (spawnedEnemies.Count >= maxEnemies) return;
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (Vector3.Distance(player.position, point.position) < 15f)
            return;

        GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.player = player;
    }
}