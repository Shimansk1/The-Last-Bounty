using UnityEngine;
using TransformSaveLoadSystem;
using UnityEngine.InputSystem;

public class PlayerNeeds : MonoBehaviour
{
    public int maxHunger = 100;
    public int maxThirst = 100;

    public int currentHunger = 100;
    public int currentThirst = 100;

    public float hungerDecreaseInterval = 7f;
    public float thirstDecreaseInterval = 3f;

    private float hungerTimer;
    private float thirstTimer;
    private float lastDamageTime = 0f;
    private float damageCooldown = 1f;

    [SerializeField] private PlayerHealth playerHealth;

    // Nové proměnné pro heal
    public float healInterval = 3f;
    public int healAmount = 1;
    private float healTimer = 0f;

    void Start()
    {
        currentHunger = SaveGameManageris.CurrentSaveData.playerData.CurrentHunger;
        currentThirst = SaveGameManageris.CurrentSaveData.playerData.CurrentThirst;

        if (currentHunger <= 0) currentHunger = maxHunger;
        if (currentThirst <= 0) currentThirst = maxThirst;
    }

    void Update()
    {
        hungerTimer += Time.deltaTime;
        thirstTimer += Time.deltaTime;
        healTimer += Time.deltaTime;

        if (hungerTimer >= hungerDecreaseInterval)
        {
            ModifyHunger(-1);
            hungerTimer = 0f;
        }

        if (thirstTimer >= thirstDecreaseInterval)
        {
            ModifyThirst(-1);
            thirstTimer = 0f;
        }

        if ((currentHunger <= 0 || currentThirst <= 0) && Time.time - lastDamageTime > damageCooldown)
        {
            playerHealth.TakeDamage(1);
            lastDamageTime = Time.time;
        }

        if (currentHunger > 80 && currentThirst > 80 && healTimer >= healInterval)
        {
            playerHealth.Heal(healAmount); 
            healTimer = 0f;
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            GetComponent<Canteen>().Drink();
            TutorialManager tutorial = FindObjectOfType<TutorialManager>();
            if (tutorial != null)
                tutorial.MarkStepComplete("drinkCanteen");
        }

        SaveGameManageris.CurrentSaveData.playerData.CurrentHunger = currentHunger;
        SaveGameManageris.CurrentSaveData.playerData.CurrentThirst = currentThirst;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterSource"))
        {
            GetComponent<Canteen>().Refill();
            TutorialManager tutorial = FindObjectOfType<TutorialManager>();
            if (tutorial != null)
                tutorial.MarkStepComplete("refillCanteen");
        }
    }

    public void ModifyHunger(int amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
    }

    public void ModifyThirst(int amount)
    {
        currentThirst = Mathf.Clamp(currentThirst + amount, 0, maxThirst);
    }
}
