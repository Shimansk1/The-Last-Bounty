using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogPanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogText;
    public Transform responseButtonContainer;
    public GameObject responseButtonPrefab;

    private NPCController currentNPC;
    private PlayerMovementScript player;
    private Interactor playerInteractor;
    private MouseLook mouseLook;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        dialogPanel.SetActive(false);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovementScript>();
        playerInteractor = FindObjectOfType<Interactor>();
        mouseLook = FindObjectOfType<MouseLook>();
    }

    private void Update()
    {
        if (dialogPanel != null && dialogPanel.activeSelf)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseDialog(true);
            }
        }
    }

    public void StartDialog(NPCController npc, DialogNode node)
    {
        currentNPC = npc;
        dialogPanel.SetActive(true);
        npcNameText.text = npc.npcName;

        if (player != null) player.canMove = false;
        if (mouseLook != null) mouseLook.canMove = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ShowNode(node);
    }

    void ShowNode(DialogNode node)
    {
        dialogText.text = node.dialogText;

        PlayerInventoryHolder inventory = FindObjectOfType<PlayerInventoryHolder>();

        foreach (Transform child in responseButtonContainer) Destroy(child.gameObject);

        foreach (var response in node.responses)
        {
            // Kontrola Itemu
            if (response.requiredItemToClick != null)
            {
                if (inventory == null || !inventory.HasItem(response.requiredItemToClick))
                {
                    continue;
                }
            }

            // NOVÉ: Kontrola Reputace
            if (response.requiredReputation > 0f)
            {
                if (ReputationManager.Instance == null || ReputationManager.Instance.GetReputation(response.reputationCity) < response.requiredReputation)
                {
                    continue; // Hráè nemá dost reputace, tlaèítko se neukáže
                }
            }

            GameObject btn = Instantiate(responseButtonPrefab, responseButtonContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (response.removesItemOnSubmit && response.requiredItemToClick != null)
                {
                    if (inventory != null) inventory.RemoveFromInventory(response.requiredItemToClick, 1);
                }

                if (response.advancesStory && MainStoryManager.Instance != null)
                {
                    MainStoryManager.Instance.AdvanceStory(response.storyStateToSet);
                }

                if (response.isExit)
                {
                    CloseDialog(true);
                }
                else if (response.triggersShop)
                {
                    NPCController shopNPC = currentNPC;
                    CloseDialog(false);
                    if (shopNPC != null) shopNPC.OpenShop();
                }
                else if (response.triggersDuel)
                {
                    NPCController duelNPC = currentNPC;
                    CloseDialog(false);
                    if (duelNPC != null && DuelManager.Instance != null)
                    {
                        DuelManager.Instance.StartDuelSetup(duelNPC);
                    }
                }
                else if (response.nextNode != null)
                {
                    ShowNode(response.nextNode);
                }
            });
        }
    }

    public void CloseDialog(bool fullyExit = true)
    {
        dialogPanel.SetActive(false);

        if (fullyExit)
        {
            if (player != null) player.canMove = true;
            if (mouseLook != null) mouseLook.canMove = true;

            if (playerInteractor != null)
            {
                playerInteractor.EndInteraction();
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (currentNPC != null)
            {
                currentNPC.EndInteraction();
                currentNPC = null;
            }
        }
        else
        {
            currentNPC = null;
        }
    }
}