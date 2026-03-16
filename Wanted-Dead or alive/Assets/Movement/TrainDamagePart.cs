using UnityEngine;

public class TrainDamagePart : MonoBehaviour
{
    private SmartTrain locomotive;
    private TrainWagon wagon;

    void Start()
    {
        locomotive = GetComponentInParent<SmartTrain>();
        wagon = GetComponentInParent<TrainWagon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (locomotive != null && !locomotive.isStopped)
            {
                CheckAndKill(other);
            }
            else if (wagon != null && wagon.locomotive != null && !wagon.locomotive.isStopped)
            {
                CheckAndKill(other);
            }
        }
    }

    void CheckAndKill(Collider other)
    {
        if (other.transform.root != transform.root)
        {
            PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(999, true);
            }
        }
    }
}