using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 100f;
    public LayerMask enemyLayer;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, speed * Time.deltaTime, enemyLayer))
        {
            Destroy(gameObject);
        }
    }
}
