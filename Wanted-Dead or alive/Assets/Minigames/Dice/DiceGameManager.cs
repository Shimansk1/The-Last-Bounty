using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DiceGameManager : MonoBehaviour
{
    public static DiceGameManager Instance;

    [Header("UI Panely")]
    public GameObject startMenuUI;
    public GameObject gameHUD;
    public GameObject resultMenuUI;

    [Header("UI Texty a Tlacitka")]
    public Text statusText;
    public Text resultText;
    public Button rollButton;

    [Header("UI Sazky")]
    public Text currentMoneyText;
    public Text currentBetText;
    public Button startButton;

    [Header("Hrac a Omezeni pohybu")]
    public MouseLook mouseLook;
    public CharacterController playerController;
    private PlayerInventoryHolder playerInventory;

    [Header("Nastaveni Kostek a Sazek")]
    public int currentBet = 10;
    public int betStep = 10;
    public GameObject dicePrefab;
    public int numberOfDice = 3;
    public float throwForce = 5f;
    public float rollTorque = 50f;
    public float reputationReward = 1f;

    [Header("Pribehove Odmeny")]
    public InventoryItemData chapter2QuestItem; // SEM PØETÁHNEŠ DOPIS V UNITY

    // LOKÁLNÍ PROMÌNNÉ
    private Transform currentThrowPoint;
    private CityName currentCity;
    private bool currentGivesReputation;
    private bool currentIsQuestTarget;

    private List<Dice> activeDice = new List<Dice>();
    private bool isRolling = false;
    private int opponentScore = 0;

    private void Awake()
    {
        Instance = this;
        startMenuUI.SetActive(false);
        gameHUD.SetActive(false);
        resultMenuUI.SetActive(false);
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventoryHolder>();
    }

    public void OpenMenu(Transform tableThrowPoint, CityName city, bool givesRep, bool isQuestTarget = false)
    {
        currentThrowPoint = tableThrowPoint;
        currentCity = city;
        currentGivesReputation = givesRep;
        currentIsQuestTarget = isQuestTarget;

        if (mouseLook != null) mouseLook.canMove = false;
        currentBet = betStep;
        UpdateMoneyUI();
        startMenuUI.SetActive(true);
    }

    public void IncreaseBet()
    {
        if (playerInventory != null && currentBet + betStep <= playerInventory.CurrentMoney)
        {
            currentBet += betStep;
            UpdateMoneyUI();
        }
    }

    public void DecreaseBet()
    {
        if (currentBet - betStep > 0)
        {
            currentBet -= betStep;
            UpdateMoneyUI();
        }
    }

    private void UpdateMoneyUI()
    {
        if (playerInventory != null)
        {
            currentMoneyText.text = "Tvoje penize: " + playerInventory.CurrentMoney + " $";
            startButton.interactable = (playerInventory.CurrentMoney >= currentBet);
        }
        currentBetText.text = "Sazka: " + currentBet + " $";
    }

    public void ExitMenu()
    {
        startMenuUI.SetActive(false);
        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<Interactor>().SendMessage("EndInteraction");
    }

    public void StartGame()
    {
        if (playerInventory == null || !playerInventory.SpendMoney(currentBet)) return;

        startMenuUI.SetActive(false);
        gameHUD.SetActive(true);

        if (playerController != null) playerController.enabled = false;
        if (mouseLook != null) mouseLook.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(OpponentTurnRoutine());
    }

    private IEnumerator OpponentTurnRoutine()
    {
        isRolling = true;
        rollButton.interactable = false;
        statusText.text = "Souper hazi...";

        ThrowDicePhysically();

        yield return new WaitUntil(AreAllDiceStopped);
        yield return new WaitForSeconds(1.5f);

        opponentScore = 0;
        foreach (Dice d in activeDice) opponentScore += d.diceValue;

        ClearTable();

        statusText.text = $"Souper hodil {opponentScore}.\nJsi na rade! Klikni na HODIT.";
        isRolling = false;
        rollButton.interactable = true;
    }

    public void RollDice()
    {
        if (isRolling) return;
        StartCoroutine(PlayerTurnRoutine());
    }

    private IEnumerator PlayerTurnRoutine()
    {
        isRolling = true;
        rollButton.interactable = false;
        statusText.text = "Tvoje kostky leti...";

        ThrowDicePhysically();

        yield return new WaitUntil(AreAllDiceStopped);
        yield return new WaitForSeconds(1.5f);

        EvaluateResult();
    }

    private void ThrowDicePhysically()
    {
        if (currentThrowPoint == null || dicePrefab == null) return;

        ClearTable();

        for (int i = 0; i < numberOfDice; i++)
        {
            Vector3 spawnPos = currentThrowPoint.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0, 0.5f), Random.Range(-0.2f, 0.2f));
            GameObject newDice = Instantiate(dicePrefab, spawnPos, Random.rotation);

            Rigidbody rb = newDice.GetComponent<Rigidbody>();
            Dice diceScript = newDice.GetComponent<Dice>();

            if (rb == null || diceScript == null) return;

            Vector3 force = (currentThrowPoint.forward + Vector3.down * 0.5f).normalized * throwForce;
            rb.AddForce(force, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * rollTorque, ForceMode.Impulse);

            activeDice.Add(diceScript);
        }
    }

    private void ClearTable()
    {
        foreach (Dice d in activeDice) if (d != null) Destroy(d.gameObject);
        activeDice.Clear();
    }

    private bool AreAllDiceStopped()
    {
        foreach (Dice d in activeDice)
        {
            if (!d.hasStopped || d.diceValue == 0) return false;
        }
        return true;
    }

    private void EvaluateResult()
    {
        int myScore = 0;
        foreach (Dice d in activeDice) myScore += d.diceValue;

        gameHUD.SetActive(false);
        resultMenuUI.SetActive(true);

        if (myScore > opponentScore)
        {
            int vyhra = currentBet * 2;
            if (playerInventory != null) playerInventory.AddMoney(vyhra);

            string repText = "";
            if (currentGivesReputation && ReputationManager.Instance != null)
            {
                ReputationManager.Instance.AddReputation(currentCity, reputationReward);
                repText = $" a +{reputationReward}% Reputace";
            }

            string storyText = "";

            // KONTROLA PØÍBÌHU A PØEDÁNÍ PØEDMÌTU
            if (currentIsQuestTarget && MainStoryManager.Instance != null && MainStoryManager.Instance.currentState == StoryProgress.Kapitola2_FalesnyHrac)
            {
                MainStoryManager.Instance.AdvanceStory(StoryProgress.Kapitola3_UcenecAHodinky);
                storyText = "\n\nZISKAL JSI: Zasifrovany dopis!";

                if (chapter2QuestItem != null && playerInventory != null)
                {
                    playerInventory.AddToInventory(chapter2QuestItem, 1);
                }
            }

            resultText.text = $"VYHRA!\n\nSouper: {opponentScore}\nTy: {myScore}\n\nVyhral jsi {vyhra} ${repText}{storyText}";
        }
        else if (myScore < opponentScore)
        {
            resultText.text = $"PROHRA!\n\nSouper: {opponentScore}\nTy: {myScore}\n\nPrisel jsi o sazku.";
        }
        else
        {
            if (playerInventory != null) playerInventory.AddMoney(currentBet);
            resultText.text = $"REMIZA!\n\nSouper: {opponentScore}\nTy: {myScore}\n\nSazka {currentBet} $ se vraci.";
        }

        isRolling = false;
    }

    public void CloseResultMenu()
    {
        resultMenuUI.SetActive(false);
        ClearTable();

        if (playerController != null) playerController.enabled = true;
        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<Interactor>().SendMessage("EndInteraction");
    }
}