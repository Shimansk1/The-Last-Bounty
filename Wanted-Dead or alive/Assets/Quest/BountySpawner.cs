using UnityEngine;
using System.Collections.Generic;

public class BountySpawner : MonoBehaviour
{
    public static BountySpawner Instance;

    [System.Serializable]
    public struct CitySpawnData
    {
        public CityName city;
        public Transform[] points;
    }

    [Header("Spawn Nastavení")]
    public List<CitySpawnData> citySpawns;

    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            var movement = FindObjectOfType<CharacterController>();
            if (movement != null) playerTransform = movement.transform;
        }
    }

    public void SpawnBountyTarget(WantedContract contract)
    {
        if (contract.targetPrefab == null) return;

        Transform[] validPoints = null;

        foreach (var data in citySpawns)
        {
            if (data.city == contract.targetCity)
            {
                validPoints = data.points;
                break;
            }
        }

        if (validPoints == null || validPoints.Length == 0) return;

        Transform spawnPoint = validPoints[Random.Range(0, validPoints.Length)];

        GameObject enemyObj = Instantiate(contract.targetPrefab, spawnPoint.position, spawnPoint.rotation);

        BountyTarget targetScript = enemyObj.AddComponent<BountyTarget>();
        targetScript.myContract = contract;

        EnemyAI ai = enemyObj.GetComponent<EnemyAI>();
        if (ai != null && playerTransform != null)
        {
            ai.player = playerTransform;
        }
    }
}