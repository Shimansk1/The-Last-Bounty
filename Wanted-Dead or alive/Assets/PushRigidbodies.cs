using UnityEngine;

public class PushRigidbodies : MonoBehaviour
{
    [Tooltip("Jakou silou bude postava strkat do objektù")]
    public float pushPower = 2.0f;

    // Tahle funkce se v Unity zavolá sama, když CharacterController do nìèeho narazí
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Zkontrolujeme, jestli jsme narazili na fyzikální objekt (Rigidbody), který se mùže hýbat
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Nechceme strkat do vìcí, po kterých zrovna šlapeme (podlaha)
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Vypoèítáme smìr síly (jen dopøedu/dozadu/do stran, ignorujeme výšku)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Aplikujeme sílu na dveøe
        body.velocity = pushDir * pushPower;
    }
}