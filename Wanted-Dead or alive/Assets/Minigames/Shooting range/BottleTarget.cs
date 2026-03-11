using UnityEngine;

public class BottleTarget : MonoBehaviour
{
    [Header("Modely a Efekty")]
    [Tooltip("Sem pøetáhni PREFAB rozbité flašky (ta, co má 2 èásti)")]
    public GameObject brokenBottlePrefab;

    [Tooltip("Sem pøetáhni svùj novı Particle System (støípky)")]
    public GameObject shatterVFX;

    [Header("Fyzika a Úklid")]
    public float hitForce = 300f;  // Jak moc vršek odletí
    public float cleanupTime = 3f; // Za jak dlouho støepy zmizí

    // Tuhle funkci zavolá tvùj skript na støelbu (Raycast)
    public void Shatter(Vector3 hitPoint, Vector3 hitDirection)
    {
        // 1. Spawne rozbitou flašku pøesnì tam, kde je ta pùvodní celá
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, transform.position, transform.rotation);

        // 2. Spawne vizuální efekt rozbitého skla
        if (shatterVFX != null)
        {
            GameObject vfx = Instantiate(shatterVFX, hitPoint, Quaternion.identity);
            Destroy(vfx, cleanupTime); // Znièí efekt ze scény, aby nezabíral pamì
        }

        // 3. Najde Rigidbody na rozbité flašce (musíš ho dát na ten odštìpenı vršek v prefabu!)
        Rigidbody[] pieces = brokenBottle.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in pieces)
        {
            // Vypne kinematiku, kdyby náhodou byla zapnutá
            rb.isKinematic = false;

            // Odmrští vršek ve smìru støely a pøidá trochu rotace, a se toèí
            rb.AddForce((hitDirection + Vector3.up * 0.5f) * hitForce);
            rb.AddTorque(Random.insideUnitSphere * 100f);
        }

        // 4. Nastaví èasovaè na smazání rozbitıch kouskù
        Destroy(brokenBottle, cleanupTime);

        // 5. Znièí pùvodní celou flašku
        Destroy(gameObject);
    }
}