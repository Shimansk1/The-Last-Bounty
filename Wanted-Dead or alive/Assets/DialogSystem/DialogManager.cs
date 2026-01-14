using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance; // Jednoduchý singleton

    [Header("UI Elements")]
    public GameObject dialogPanel; // Celé okno dialogu
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogText;
    public Transform responseButtonContainer; // Kam se budou spawnovat tlaèítka
    public GameObject responseButtonPrefab;   // Prefab tlaèítka

    private NPCController currentNPC;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        dialogPanel.SetActive(false);
    }

    public void StartDialog(NPCController npc, DialogNode node)
    {
        currentNPC = npc;
        dialogPanel.SetActive(true);
        npcNameText.text = npc.npcName;

        ShowNode(node);
    }

    void ShowNode(DialogNode node)
    {
        dialogText.text = node.dialogText;

        // Vyèistit stará tlaèítka
        foreach (Transform child in responseButtonContainer) Destroy(child.gameObject);

        // Vytvoøit nová tlaèítka podle možností
        foreach (var response in node.responses)
        {
            GameObject btn = Instantiate(responseButtonPrefab, responseButtonContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Nastavení kliknutí
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (response.isExit)
                {
                    CloseDialog();
                }
                else if (response.triggersShop)
                {
                    CloseDialog();
                    currentNPC.OpenShop(); // Otevøe obchod
                }
                else if (response.nextNode != null)
                {
                    ShowNode(response.nextNode); // Jde na další èást dialogu
                }
            });
        }
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);

        // Dùležité: Øíct hráèi, že interakce skonèila, aby se schovala myš
        // Musíš najít referenci na hráèe/Interactor a zavolat EndInteraction, 
        // nebo v Interactoru hlídat stisk ESC (což tam už máš).

        if (currentNPC != null)
        {
            currentNPC.EndInteraction();
            currentNPC = null;
        }
    }
}