using UnityEngine;

public enum ContractType
{
    Main,
    Side
}

[CreateAssetMenu(fileName = "NewContract", menuName = "TheLastBounty/Wanted Contract")]
public class WantedContract : ScriptableObject
{
    public string contractName;
    [TextArea] public string description;

    public ContractType contractType;
    public CityName targetCity; 

    public GameObject targetPrefab;
    public int reward;
}