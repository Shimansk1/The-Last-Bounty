using UnityEngine;

public class TumbleweedChovani : MonoBehaviour
{
    [Header("Nastavení Vìtru a Skoku")]
    public float minRychlost = 1.5f;
    public float maxRychlost = 3f;
    public float silaRotace = 2f;

    private Rigidbody rb;
    private float aktualniRychlost;
    private Vector3 cilovaPozice;
    private bool mamCil = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aktualniRychlost = Random.Range(minRychlost, maxRychlost);
    }

    // Tuhle funkci zavolá Spawner hned po vytvoøení
    public void NastavCil(Vector3 cil)
    {
        cilovaPozice = cil;
        mamCil = true;

        // Dáme mu malı šouchanec nahoru do zaèátku, a hned zaène skákat
        rb.AddForce(Vector3.up * Random.Range(2f, 4f), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (!mamCil) return; // Pokud neví kam jet, nic nedìlá

        // 1. Spoèítáme smìr k cíli
        Vector3 smerKCili = (cilovaPozice - transform.position);
        smerKCili.y = 0; // Ignorujeme vıšku, a se nesnaí letìt do vzduchu nebo pod zem
        smerKCili.Normalize(); // Udìláme z toho èistı smìr

        // 2. Aplikujeme rychlost v tomto smìru, ale Y (skákání) necháme na fyzice
        Vector3 novaRychlost = smerKCili * aktualniRychlost;
        rb.velocity = new Vector3(novaRychlost.x, rb.velocity.y, novaRychlost.z);

        // 3. Pøidáme kutálení
        rb.AddTorque(new Vector3(0, 0, -1) * silaRotace, ForceMode.Force);
    }
}