using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 100f;
    public LayerMask enemyLayer;

    // NOVÉ: Jak daleko má kulka doletìt, než zmizí
    public float maxDistance;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceThisFrame = speed * Time.deltaTime;

        // 1. Kontrola kolize se zdí (pro jistotu, kdyby se nìco pøipletlo do cesty)
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distanceThisFrame, enemyLayer))
        {
            Destroy(gameObject);
            return;
        }

        // 2. Pohyb
        transform.position += direction * distanceThisFrame;

        // 3. NOVÉ: Kontrola uletìné vzdálenosti
        // Pokud jsme uletìli vzdálenost, kterou nám urèil WeaponHandler, konèíme.
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}