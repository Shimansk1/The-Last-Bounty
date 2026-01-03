using UnityEngine;
using UnityEngine.UI;
using TransformSaveLoadSystem;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private MouseLook mouseLook;
    public int maxHealth = 100;
    public int currentHealth = 100;

    public float fallThreshold = 12f;
    public float damageMultiplier = 20f;

    public Image damageEffect;
    public GameObject deathScreen;
    [SerializeField] public PlayerMovementScript playerMovementScript;
    private SaveGameManager saveGameManager;

    public bool isDead = false;
    private Vector3 lastGroundedPosition;

    private bool damageCooldown = false;
    public float damageCooldownDuration = 2f;

    private void Start()
    {
        saveGameManager = FindObjectOfType<SaveGameManager>();
        if (saveGameManager == null) Debug.LogError("SaveGameManager není ve scénì!");

        lastGroundedPosition = transform.position;
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    void Update()
    {
        if (isDead) return;

        if (IsGrounded())
        {
            float fallDistance = lastGroundedPosition.y - transform.position.y;
            if (fallDistance > fallThreshold)
            {
                int fallDamage = (int)((fallDistance - fallThreshold) * damageMultiplier);
                TakeDamage(fallDamage, true);
            }
            lastGroundedPosition = transform.position;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.1f);
    }

    public void TakeDamage(int damage, bool ignoreCooldown = false)
    {
        if (currentHealth > 0)
        {
        if (isDead) return;
        if (!ignoreCooldown && damageCooldown) return;

        currentHealth -= damage;
        //UpdateSaveData();
        StartCoroutine(FadeDamageEffect());

        if (!ignoreCooldown)
            StartCoroutine(DamageCooldown());

        if (currentHealth <= 0)
        {
            Die();
        }
        } 
    }

    private IEnumerator DamageCooldown()
    {
        damageCooldown = true;
        yield return new WaitForSeconds(damageCooldownDuration);
        damageCooldown = false;
    }

    private void UpdateSaveData()
    {
        SaveGameManageris.CurrentSaveData.playerData.CurrentHealth = currentHealth;
    }

    IEnumerator FadeDamageEffect()
    {
        damageEffect.color = new Color(1f, 0f, 0f, 0.8f);
        yield return new WaitForSeconds(0.5f);

        float fadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0.8f, 0f, elapsedTime / fadeDuration);
            damageEffect.color = new Color(1f, 0f, 0f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        damageEffect.color = new Color(1f, 0f, 0f, 0f);
    }

    void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseLook.canMove = false;
        isDead = true;
        deathScreen.SetActive(true);
        playerMovementScript.enabled = false;
        saveGameManager.respawned = false;
    }
}
