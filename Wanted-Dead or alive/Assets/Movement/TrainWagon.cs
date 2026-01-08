using UnityEngine;
using System.Collections.Generic;

public class TrainWagon : MonoBehaviour
{
    [Header("Napojení")]
    public SmartTrain locomotive;
    public float wagonDistance = 4.0f;
    public float turnSpeed = 5f;
    public float lookAheadDistance = 1.0f;

    [Header("Pozice Hráèe")]
    public Vector3 seatPosition = new Vector3(0, 1.1f, 0);

    private PlayerMovementScript myScript;
    private GameObject playerObj;
    private bool isRiding = false;

    // NOVÉ: Èasovaè, aby se E nezmáèklo 2x
    private float exitCooldown = 0f;

    void LateUpdate()
    {
        // 1. POHYB VAGÓNU (Beze zmìny)
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
            // Držíme jen pozici (aby neklouzal), ale rotaci necháme volnou (aby se mohl rozhlížet)
            playerObj.transform.localPosition = seatPosition;
        }

        // --- NOVÉ: ODEÈÍTÁNÍ ÈASU ---
        if (exitCooldown > 0)
        {
            exitCooldown -= Time.deltaTime;
        }

        // 3. VÝSTUP NA E (Jen pokud ubìhl èasovaè!)
        if (isRiding && Input.GetKeyDown(KeyCode.E) && exitCooldown <= 0)
        {
            UnlockPlayer();
        }
    }

    public void LockPlayer(GameObject player)
    {
        Debug.Log("ZAMYKÁM HRÁÈE...");
        playerObj = player;
        isRiding = true;

        // --- NOVÉ: NASTAVÍME ÈASOVAÈ NA 1 VTEØINU ---
        // Tím zajistíme, že ten samý stisk E tì hned nevyhodí ven
        exitCooldown = 1.0f;

        // Najdeme a vypneme skript
        myScript = player.GetComponent<PlayerMovementScript>();
        if (myScript != null)
        {
            myScript.enabled = false;
            Debug.Log("SKRIPT VYPNUT.");
        }

        // Pøilepíme
        player.transform.SetParent(this.transform);
        player.transform.localPosition = seatPosition;
        player.transform.localRotation = Quaternion.identity;
    }

    public void UnlockPlayer()
    {
        if (playerObj == null) return;
        isRiding = false;

        playerObj.transform.SetParent(null);

        if (myScript != null)
        {
            myScript.enabled = true;
            Debug.Log("SKRIPT ZAPNUT.");
        }

        playerObj = null;
        myScript = null;
    }

    // Matematika (Beze zmìny)
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