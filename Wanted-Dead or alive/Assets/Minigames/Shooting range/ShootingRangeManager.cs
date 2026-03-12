using UnityEngine;
using UnityEngine.UI;

public class ShootingRangeManager : MonoBehaviour
{
    public static ShootingRangeManager Instance;

    [Header("UI Panely")]
    public GameObject startMenuUI;
    public GameObject gameHUD;
    public GameObject resultMenuUI; // NOVÉ: Panel s výsledkem na konci hry

    [Header("UI Texty (Legacy)")]
    public Text highScoreText;
    public Text currentScoreText;
    public Text timerText;
    public Text finalScoreText;    // NOVÉ: Text, který ukáže skóre na konci

    [Header("Hráè a Omezení pohybu")]
    public MouseLook mouseLook;
    public CharacterController playerController;

    [Header("Spawner Flašek")]
    public GameObject bottlePrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 1.5f;

    [Header("Nastavení Minihry")]
    public float gameDuration = 30f;

    private float timer;
    private float bottleTimer;
    private int currentScore;
    private bool isPlaying = false;

    private void Awake()
    {
        Instance = this;
        startMenuUI.SetActive(false);
        gameHUD.SetActive(false);
        resultMenuUI.SetActive(false); // Ujistíme se, že je panel na zaèátku vypnutý
    }

    public void OpenMenu()
    {
        int bestScore = PlayerPrefs.GetInt("ShootingHighScore", 0);
        highScoreText.text = "Highest Score: " + bestScore;

        if (mouseLook != null) mouseLook.canMove = false;
        startMenuUI.SetActive(true);
    }

    public void CloseMenu()
    {
        startMenuUI.SetActive(false);

        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<Interactor>().SendMessage("EndInteraction");
    }

    public void StartGame()
    {
        startMenuUI.SetActive(false);
        gameHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mouseLook != null) mouseLook.canMove = true;
        if (playerController != null) playerController.enabled = false;

        currentScore = 0;
        timer = gameDuration;
        bottleTimer = 0f;
        isPlaying = true;

        UpdateUI();
    }

    private void Update()
    {
        if (!isPlaying) return;

        timer -= Time.deltaTime;
        timerText.text = timer.ToString("F1") + "s";

        bottleTimer -= Time.deltaTime;
        if (bottleTimer <= 0)
        {
            SpawnBottle();
            bottleTimer = spawnInterval;
        }

        if (timer <= 0)
        {
            EndGame(); // Èas vypršel, spustíme konec
        }
    }

    private void SpawnBottle()
    {
        if (spawnPoints.Length == 0 || bottlePrefab == null) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        GameObject newBottle = Instantiate(bottlePrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(newBottle, spawnInterval + 0.5f);
    }

    public void AddScore()
    {
        if (!isPlaying) return;

        currentScore++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        currentScoreText.text = "Score: " + currentScore;
    }

    private void EndGame()
    {
        isPlaying = false;
        gameHUD.SetActive(false); // Vypneme HUD s èasomírou

        // Uložení Highscore
        int bestScore = PlayerPrefs.GetInt("ShootingHighScore", 0);
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("ShootingHighScore", currentScore);
            PlayerPrefs.Save();
        }

        // --- NOVÉ: Zobrazení výsledku ---
        finalScoreText.text = "Konecne skore: " + currentScore;
        resultMenuUI.SetActive(true);

        // Odemkneme kurzor a zamkneme kameru, aby hráè mohl kliknout na "Odejít"
        if (mouseLook != null) mouseLook.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Tuhle funkci pøidáš na tlaèítko "Pokraèovat/Zavøít" v tom novém Result panelu
    public void CloseResultMenu()
    {
        resultMenuUI.SetActive(false);

        // Vrátíme chod hry úplnì do normálu
        if (playerController != null) playerController.enabled = true;
        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Odstraní interakci
        FindObjectOfType<Interactor>().SendMessage("EndInteraction");
    }
}