using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Loot nastavení")]
    public GameObject[] lootItems;
    public int minLoot = 0;
    public int maxLoot = 2;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        DropLoot();

        TutorialManager tutorial = FindObjectOfType<TutorialManager>();
        if (tutorial != null)
            tutorial.MarkStepComplete("killEnemy");

        Destroy(gameObject);
    }

    void DropLoot()
    {
        if (lootItems == null || lootItems.Length == 0) return;

        int lootCount = Random.Range(minLoot, maxLoot + 1);

        for (int i = 0; i < lootCount; i++)
        {
            GameObject loot = lootItems[Random.Range(0, lootItems.Length)];
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            Instantiate(loot, spawnPosition, Quaternion.identity);
        }
    }
}
