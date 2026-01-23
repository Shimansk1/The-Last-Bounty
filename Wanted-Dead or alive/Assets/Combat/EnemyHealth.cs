using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider;

    [Header("Loot nastavení")]
    public GameObject[] lootItems;
    public int minLoot = 0;
    public int maxLoot = 2;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // --- NOVÉ: KONTROLA BOUNTY ---
        // Zjistíme, jestli jsem byl cílem kontraktu
        BountyTarget bountyInfo = GetComponent<BountyTarget>();
        if (bountyInfo != null && bountyInfo.myContract != null)
        {
            // Pokud ano, nahlásíme smrt Trackeru
            if (WantedQuestTracker.Instance != null)
            {
                WantedQuestTracker.Instance.NotifyEnemyDeath(bountyInfo.myContract);
            }
        }
        // -----------------------------

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