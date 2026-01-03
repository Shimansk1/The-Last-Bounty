using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Reference")]
    public Transform player;

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;         // základní prefab bandity
    public Transform[] spawnPoints;        // místa, kde se mohou spawnout
    public float spawnInterval = 20f;      // jak èasto se spawnou (v sekundách)
    public int maxEnemies = 10;            // limit na poèet nepøátel ve scénì

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

        // èistí seznam od mrtvých/enemákù co zmizeli
        spawnedEnemies.RemoveAll(e => e == null);
    }

    void TrySpawnEnemy()
    {
        if (spawnedEnemies.Count >= maxEnemies) return;
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ne spawnuj hned vedle hráèe
        if (Vector3.Distance(player.position, point.position) < 15f)
            return;

        GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);

        // Pøedání reference na hráèe AIèku
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.player = player;

        Debug.Log($"Spawned enemy at {point.position}");
    }
}
