using UnityEngine;
using System.Collections.Generic;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance { get; private set; }

    private Dictionary<CityName, float> cityReputations = new Dictionary<CityName, float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeReputations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeReputations()
    {
        foreach (CityName city in System.Enum.GetValues(typeof(CityName)))
        {
            cityReputations[city] = 0f;
        }
    }

    public void AddReputation(CityName city, float amount)
    {
        if (cityReputations.ContainsKey(city))
        {
            cityReputations[city] = Mathf.Clamp(cityReputations[city] + amount, 0f, 100f);
        }
    }

    public float GetReputation(CityName city)
    {
        if (cityReputations.ContainsKey(city))
        {
            return cityReputations[city];
        }
        return 0f;
    }
}