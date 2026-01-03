using UnityEngine;

public class EnemyLootDropper : MonoBehaviour
{
    [Header("Mo�n� looty")]
    public GameObject[] lootPrefabs;
    public float dropChance = 0.5f;

    public void DropLoot()
    {
        if (lootPrefabs.Length == 0) return;

        if (Random.value <= dropChance)
        {
            GameObject selectedLoot = lootPrefabs[Random.Range(0, lootPrefabs.Length)];
            Instantiate(selectedLoot, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }
    }
}
