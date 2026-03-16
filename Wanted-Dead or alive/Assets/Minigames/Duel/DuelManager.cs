using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class DuelManager : MonoBehaviour
{
    public static DuelManager Instance;

    [Header("UI")]
    public GameObject duelUI;
    public Text statusText;
    public Image flashPanel;

    [Header("Kamery a Hrac")]
    public Camera mainCamera;
    public MouseLook mouseLook;
    public CharacterController playerController;
    private NPCController opponentNPC;
    private Camera activeDuelCam;

    [Header("Audio a VFX (Efekty)")]
    public AudioSource duelAudioSource;
    public AudioClip tensionMusic;
    public AudioClip bellSignalSound;
    public AudioClip gunshotSound;
    public GameObject bloodParticlePrefab;

    [Header("Nastaveni Duelu")]
    public float minWaitTime = 3f;
    public float maxWaitTime = 7f;
    public float timeToShoot = 0.6f;

    [Tooltip("Kolik reputace ziskas za normalni nahodny duel")]
    public float defaultReputationReward = 3f;

    [Tooltip("Kolik reputace ziskas, kdyz je to hlavni pribehovy boss (Prava Ruka)")]
    public float storyBossReputationReward = 50f;

    private CityName currentCity;

    private enum DuelState { Inactive, Setup, Waiting, Signal, Finished }
    private DuelState currentState = DuelState.Inactive;

    private float waitTimer = 0f;
    private float reactionTimer = 0f;

    private void Awake()
    {
        Instance = this;
        if (duelUI != null) duelUI.SetActive(false);
    }

    public void StartDuelSetup(NPCController npc)
    {
        opponentNPC = npc;

        if (npc != null)
        {
            currentCity = npc.currentCity;
        }

        currentState = DuelState.Setup;

        if (playerController != null) playerController.enabled = false;
        if (mouseLook != null) mouseLook.canMove = false;

        Transform standPoint = npc.transform.Find("PlayerStandPoint");
        if (standPoint != null)
        {
            playerController.transform.position = standPoint.position;
            playerController.transform.rotation = standPoint.rotation;
        }

        Transform duelCam = npc.transform.Find("DuelCamera");
        if (duelCam != null)
        {
            activeDuelCam = duelCam.GetComponent<Camera>();
            mainCamera.gameObject.SetActive(false);
            activeDuelCam.gameObject.SetActive(true);
        }

        duelUI.SetActive(true);
        statusText.text = "PRIPRAV SE К DUELU!\nStiskni a DRZ prave tlacitko mysi...";

        StartCoroutine(DuelRoutine());
    }

    private IEnumerator DuelRoutine()
    {
        yield return new WaitUntil(() => Input.GetMouseButton(1));

        currentState = DuelState.Waiting;
        statusText.text = "Cekej na signal...";

        if (duelAudioSource != null && tensionMusic != null)
        {
            duelAudioSource.clip = tensionMusic;
            duelAudioSource.Play();
        }

        waitTimer = Random.Range(minWaitTime, maxWaitTime);

        while (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;

            if (!Input.GetMouseButton(1))
            {
                LoseDuel("FAUL! Tasil jsi moc brzo.");
                yield break;
            }
            yield return null;
        }

        currentState = DuelState.Signal;
        statusText.text = "TAS!\n(Pust prave a klikni leve!)";

        if (duelAudioSource != null) duelAudioSource.Stop();
        if (duelAudioSource != null && bellSignalSound != null) duelAudioSource.PlayOneShot(bellSignalSound);

        reactionTimer = timeToShoot;

        while (reactionTimer > 0)
        {
            reactionTimer -= Time.deltaTime;

            if (!Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
            {
                WinDuel();
                yield break;
            }
            yield return null;
        }

        LoseDuel("POMALU! Protivnik te dostal.");
    }

    private void WinDuel()
    {
        currentState = DuelState.Finished;
        StartCoroutine(FlashEffect(Color.white));

        if (duelAudioSource != null && gunshotSound != null) duelAudioSource.PlayOneShot(gunshotSound);
        if (bloodParticlePrefab != null && opponentNPC != null)
        {
            Instantiate(bloodParticlePrefab, opponentNPC.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }

        float repToGive = defaultReputationReward;
        string extraText = "";

        if (opponentNPC != null && opponentNPC.isMainStoryDuelist)
        {
            repToGive = storyBossReputationReward;

            if (MainStoryManager.Instance != null && MainStoryManager.Instance.currentState == StoryProgress.Kapitola4_PravaRukaBoss)
            {
                MainStoryManager.Instance.AdvanceStory(StoryProgress.Kapitola5_PovoleniOdSerifa);
                extraText = "\n\nPrava ruka je mrtva. Vrat se za Serifem!";
            }
        }

        if (ReputationManager.Instance != null)
        {
            ReputationManager.Instance.AddReputation(currentCity, repToGive);
        }

        statusText.text = $"VYHRA!\nZiskal jsi +{repToGive}% Reputace.{extraText}\n(Stiskni ESC pro navrat)";

        if (opponentNPC != null)
        {
            if (activeDuelCam != null) activeDuelCam.transform.SetParent(null);
            opponentNPC.transform.Rotate(-90f, 0f, 0f);
        }
    }

    private void LoseDuel(string reason)
    {
        currentState = DuelState.Finished;
        StartCoroutine(FlashEffect(Color.red));
        statusText.text = reason;

        if (duelAudioSource != null && gunshotSound != null) duelAudioSource.PlayOneShot(gunshotSound);
        if (bloodParticlePrefab != null)
        {
            Instantiate(bloodParticlePrefab, playerController.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }

        if (ReputationManager.Instance != null) ReputationManager.Instance.AddReputation(currentCity, -1f);

        Invoke(nameof(KillPlayer), 1.5f);
    }

    private void KillPlayer()
    {
        EndDuel();

        PlayerHealth ph = playerController.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(9999);
        }
    }

    public void EndDuel()
    {
        if (currentState != DuelState.Finished && currentState != DuelState.Inactive) return;

        duelUI.SetActive(false);
        currentState = DuelState.Inactive;

        mainCamera.gameObject.SetActive(true);

        if (activeDuelCam != null && activeDuelCam.transform.parent == null) Destroy(activeDuelCam.gameObject);
        if (opponentNPC != null) Destroy(opponentNPC.gameObject);

        if (playerController != null)
        {
            playerController.enabled = true;

            PlayerMovementScript pms = playerController.GetComponent<PlayerMovementScript>();
            if (pms != null) pms.canMove = true;
        }

        if (mouseLook != null) mouseLook.canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<Interactor>().SendMessage("EndInteraction");
    }

    private IEnumerator FlashEffect(Color flashColor)
    {
        if (flashPanel != null)
        {
            flashPanel.color = flashColor;
            float alpha = 1f;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * 2f;
                flashPanel.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
                yield return null;
            }
        }
    }

    private void Update()
    {
        if (currentState == DuelState.Finished && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EndDuel();
        }
    }
}