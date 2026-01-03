using UnityEngine;

public enum ContractType
{
    Main,   // Zlatý rám
    Side    // Støíbrný rám
}

[CreateAssetMenu(fileName = "NewContract", menuName = "TheLastBounty/Wanted Contract")]
public class WantedContract : ScriptableObject
{
    public string contractName;
    public string description;

    public ContractType contractType;

    public GameObject targetPrefab;   // koho spawnout
    public int reward;                // odmìna po zabití cíle
}
