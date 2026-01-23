using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        transform.LookAt(transform.position + cam.forward);
    }
}