using UnityEngine;

public class Canteen : MonoBehaviour
{
    public float waterAmount = 100f;
    public float maxWaterAmount = 100f;
    public float drinkAmount = 25f;
    public int thirstRestoreAmount = 20;

    public AudioClip drinkSound;

    private PlayerNeeds playerNeeds;
    private AudioSource audioSource;

    void Start()
    {
        playerNeeds = GetComponent<PlayerNeeds>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public bool Drink()
    {
        if (waterAmount >= drinkAmount)
        {
            waterAmount -= drinkAmount;

            if (drinkSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(drinkSound);
            }

            if (playerNeeds != null)
            {
                playerNeeds.ModifyThirst(thirstRestoreAmount);
            }

            return true;
        }

        return false;
    }

    public void Refill()
    {
        waterAmount = maxWaterAmount;
    }
}