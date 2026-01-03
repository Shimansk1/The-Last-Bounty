using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance { get; private set; }

    private float mainCityReputation = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddReputation(float amount)
    {
        mainCityReputation = Mathf.Clamp(mainCityReputation + amount, 0f, 100f);
    }

    public float GetReputation()
    {
        return mainCityReputation;
    }
}