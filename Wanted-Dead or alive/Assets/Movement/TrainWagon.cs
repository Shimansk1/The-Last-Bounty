using UnityEngine;
using System.Collections.Generic;

public class TrainWagon : MonoBehaviour
{
    [Header("Napojení")]
    public SmartTrain locomotive;
    public float wagonDistance = 4.0f;
    public float turnSpeed = 5f;
    public float lookAheadDistance = 1.0f;

    [Header("Pozice Hráèe ve vlaku")]
    public Vector3 seatPosition = new Vector3(0, 1.1f, 0);

    private PlayerMovementScript myScript;
    private GameObject playerObj;
    private bool isRiding = false;
    private float exitCooldown = 0f; // Aby tì to nevyhodilo hned po nástupu

    void LateUpdate()
    {
        // 1. POHYB VAGÓNU
        if (locomotive != null && locomotive.breadcrumbs.Count >= 2)
        {
            Vector3 myPosition = GetPointOnPath(locomotive.breadcrumbs, wagonDistance);
            transform.position = myPosition;

            float targetDist = Mathf.Max(0, wagonDistance - lookAheadDistance);
            Vector3 targetPoint = GetPointOnPath(locomotive.breadcrumbs, targetDist);
            Vector3 direction = targetPoint - transform.position;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }

        // 2. DRŽENÍ HRÁÈE
        if (isRiding && playerObj != null)
        {
            playerObj.transform.localPosition = seatPosition;
            // Rotaci necháváme volnou pro rozhlížení
        }

        // Cooldown
        if (exitCooldown > 0) exitCooldown -= Time.deltaTime;

        // 3. VÝSTUP NA E
        // Podmínka: Ubìhl èasovaè A ZÁROVEÒ vlak stojí
        if (isRiding && Input.GetKeyDown(KeyCode.E) && exitCooldown <= 0)
        {
            if (locomotive.isStopped && locomotive.currentExitPoint != null)
            {
                UnlockPlayer();
            }
            else
            {
                Debug.Log("Vlak jede (nebo není nastaven Exit Point)! Nemùžeš vystoupit.");
            }
        }
    }

    public void LockPlayer(GameObject player)
    {
        playerObj = player;
        isRiding = true;
        exitCooldown = 1.0f; // 1 sekunda pauza po nástupu

        myScript = player.GetComponent<PlayerMovementScript>();
        if (myScript != null) myScript.enabled = false;

        player.transform.SetParent(this.transform);
        player.transform.localPosition = seatPosition;
        player.transform.localRotation = Quaternion.identity;
    }

    public void UnlockPlayer()
    {
        if (playerObj == null) return;

        isRiding = false;

        // Musíme najít CharacterController, abychom ho mohli vypnout
        CharacterController cc = playerObj.GetComponent<CharacterController>();

        // 1. DÙLEŽITÉ: Vypneme CC pøed teleportem
        if (cc != null) cc.enabled = false;

        // 2. Odlepíme hráèe
        playerObj.transform.SetParent(null);

        // 3. TELEPORT NA ZASTÁVKU
        if (locomotive.currentExitPoint != null)
        {
            // Teï, když je CC vypnutý, teleport probìhne spolehlivì
            playerObj.transform.position = locomotive.currentExitPoint.position;
            playerObj.transform.rotation = locomotive.currentExitPoint.rotation;
        }

        // 4. DÙLEŽITÉ: Zapneme CC zpátky
        if (cc != null) cc.enabled = true;

        // 5. Zapneme tvùj pohybový skript
        if (myScript != null) myScript.enabled = true;

        playerObj = null;
        myScript = null;

        Debug.Log("Vystoupil jsi na zastávce.");
    }

    // Matematika (beze zmìny)
    Vector3 GetPointOnPath(List<Vector3> path, float distanceNeeded)
    {
        float distanceCovered = 0;
        for (int i = path.Count - 1; i > 0; i--)
        {
            Vector3 p1 = path[i]; Vector3 p2 = path[i - 1]; float dist = Vector3.Distance(p1, p2);
            if (distanceCovered + dist >= distanceNeeded)
            {
                float remaining = distanceNeeded - distanceCovered; float t = remaining / dist;
                return Vector3.Lerp(p1, p2, t);
            }
            distanceCovered += dist;
        }
        return path[0];
    }
}