using UnityEngine;

public class Canteen : MonoBehaviour
{
    public float waterAmount = 100f;
    public float maxWaterAmount = 100f;
    public float drinkAmount = 25f;
    public int thirstRestoreAmount = 20;

    private PlayerNeeds playerNeeds;

    void Start()
    {
        playerNeeds = GetComponent<PlayerNeeds>();
    }

    public bool Drink()
    {
        if (waterAmount >= drinkAmount)
        {
            waterAmount -= drinkAmount;

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
