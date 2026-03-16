using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    public int diceValue = 0;
    public bool hasStopped = false;
    public Transform[] sides;

    // NOVÉ: Èasovaè, aby kostka nevyhodnotila padnutí ve vzduchu
    private float stopTimer = 0f;
    public float requiredStopTime = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Pokud je kostka skoro v klidu...
        if (rb.velocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f)
        {
            stopTimer += Time.deltaTime; // ...zaèneme poèítat èas

            // Až když je v klidu déle než pùl vteøiny, vyhodnotíme ji
            if (stopTimer >= requiredStopTime && !hasStopped)
            {
                hasStopped = true;
                CalculateValue();
            }
        }
        else
        {
            // Pokud se kostka zase pohne (odraz), èasovaè se resetuje
            stopTimer = 0f;
            hasStopped = false;
            diceValue = 0;
        }
    }

    void CalculateValue()
    {
        float highestY = float.MinValue;
        Transform highestSide = null;

        foreach (Transform side in sides)
        {
            if (side.position.y > highestY)
            {
                highestY = side.position.y;
                highestSide = side;
            }
        }

        if (highestSide != null)
        {
            string name = highestSide.name;
            int foundValue = 0;

            foreach (char c in name)
            {
                if (char.IsDigit(c))
                {
                    foundValue = int.Parse(c.ToString());
                    break;
                }
            }

            if (foundValue > 0 && foundValue <= 6)
            {
                diceValue = foundValue;
            }
            else
            {
                diceValue = 1;
            }
        }
    }
}