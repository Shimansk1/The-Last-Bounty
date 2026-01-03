using UnityEngine;
using UnityEngine.UI;

public class CanteenUI : MonoBehaviour
{
    public Canteen canteen;
    public Text canteenText;

    void Update()
    {
        if (canteen != null && canteenText != null)
        {
            canteenText.text = $"Cutora: {canteen.waterAmount}/{canteen.maxWaterAmount}";
        }
    }
}
