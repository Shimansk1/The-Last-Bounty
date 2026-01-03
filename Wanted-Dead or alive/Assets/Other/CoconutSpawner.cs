using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoconutSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Víc palm, víc kokosù
    public GameObject coconutPrefab;
    public float coconutLifetime = 300f; // 5 minut

    private List<GameObject> spawnedCoconuts = new List<GameObject>();

    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            GameObject coconut = Instantiate(coconutPrefab, point.position, Quaternion.identity);
            spawnedCoconuts.Add(coconut);
            StartCoroutine(DestroyAfterTime(coconut, coconutLifetime));
        }
    }

    IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        if (obj != null)
        {
            spawnedCoconuts.Remove(obj);
            Destroy(obj);
        }
    }
}
