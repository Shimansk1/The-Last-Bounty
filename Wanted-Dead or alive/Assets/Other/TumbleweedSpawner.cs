using System.Collections;
using UnityEngine;

public class TumbleweedSpawner : MonoBehaviour
{
    [Header("Nastavení")]
    public GameObject tumbleweedPrefab;
    public Transform bodA; // Odkud vyjíždí
    public Transform bodB; // Kam smìøuje (NOVÉ)

    [Header("Èasování")]
    public float minCasCekani = 4f;
    public float maxCasCekani = 10f;

    void Start()
    {
        StartCoroutine(Spawnovani());
    }

    IEnumerator Spawnovani()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minCasCekani, maxCasCekani));

            // 1. Vytvoøíme tumbleweed
            GameObject novy = Instantiate(tumbleweedPrefab, bodA.position, Random.rotation);

            // 2. Øekneme mu, kam má jet! Získáme jeho skript a pøedáme mu pozici Bodu B
            TumbleweedChovani chovani = novy.GetComponent<TumbleweedChovani>();
            if (chovani != null)
            {
                chovani.NastavCil(bodB.position);
            }

            // 3. Za 15 vteøin ho smažeme (jistota je jistota)
            Destroy(novy, 15f);
        }
    }
}