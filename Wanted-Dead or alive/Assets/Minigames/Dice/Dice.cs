using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Stav kostky")]
    public int diceValue = 0;
    public bool hasStopped = false;

    [Tooltip("Sem pøetáhni tìch 6 prázdných objektù (1 až 6) z hierarchie kostky")]
    public Transform[] sides;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Kontrola, jestli už kostka pøestala skákat a rotovat
        if (rb.velocity.sqrMagnitude < 0.01f && rb.angularVelocity.sqrMagnitude < 0.01f)
        {
            if (!hasStopped)
            {
                hasStopped = true;
                CalculateValue();
            }
        }
        else
        {
            hasStopped = false;
            diceValue = 0; // Dokud se hejbe, hodnota je 0
        }
    }

    void CalculateValue()
    {
        float highestY = float.MinValue;
        Transform highestSide = null;

        // Projde všech 6 stran a najde tu, co je nejvýš
        foreach (Transform side in sides)
        {
            if (side.position.y > highestY)
            {
                highestY = side.position.y;
                highestSide = side;
            }
        }

        // Z názvu toho nejvyššího bodu zjistíme èíslo (proto jsme je pojmenovali 1, 2, 3...)
        if (highestSide != null)
        {
            int.TryParse(highestSide.name, out diceValue);
            Debug.Log("Kostka se zastavila na èísle: " + diceValue);
        }
    }
}