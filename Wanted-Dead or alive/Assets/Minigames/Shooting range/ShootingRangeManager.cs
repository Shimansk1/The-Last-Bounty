using UnityEngine;
using UnityEngine.UI; // Zmìna: Používáme základní UI knihovnu místo TMPro

public class ShootingRangeManager : MonoBehaviour
{
    public static ShootingRangeManager Instance;

    [Header("UI Panely")]
    public GameObject startMenuUI; // Panel s tlacitkem Hrat a Highscore
    public GameObject gameHUD;     // Panel s casovacem a skore nahore vpravo

    [Header("UI Texty (Legacy)")]
    public Text highScoreText;    // Zmìnìno z TextMeshProUGUI na Text
    public Text currentScoreText; // Zmìnìno z TextMeshProUGUI na Text
    public Text timerText;        // Zmìnìno z TextMeshProUGUI na Text

    [Header("Nastavení Minihry")]
    public float gameDuration = 30f; // Jak dlouho minihra trva

    private float timer;
    private int currentScore;
    private bool isPlaying = false;

    private void Awake()
    {
        Instance = this;
        startMenuUI.SetActive(false);
        gameHUD.SetActive(false);
    }

    public void OpenMenu()
    {
        // Nacte highscore ulozene v pocitaci (pokud neni, vrati 0)
        int bestScore = PlayerPrefs.GetInt("ShootingHighScore", 0);
        highScoreText.text = "Highest Score: " + bestScore;

        startMenuUI.SetActive(true);
    }

    public void CloseMenu()
    {
        startMenuUI.SetActive(false);
    }

    // Tuhle funkci napoj na OnClick event toho UI tlacitka "Hrát"
    public void StartGame()
    {
        startMenuUI.SetActive(false);
        gameHUD.SetActive(true);

        // Znovu zamkneme kurzor do hry, protoze ho Interactor odemkl kvuli UI
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentScore = 0;
        timer = gameDuration;
        isPlaying = true;

        UpdateUI();

        // ZDE POZDEJI PRIDAME FUNKCI NA SPAWNOVANI FLASEK
    }

    private void Update()
    {
        if (!isPlaying) return;

        timer -= Time.deltaTime;

        // Formátuje èas jako sekundy s jedním desetinným místem
        timerText.text = timer.ToString("F1") + "s";

        if (timer <= 0)
        {
            EndGame();
        }
    }

    public void AddScore()
    {
        if (!isPlaying) return; // Pokud hra nebezi, nedavej body

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
        gameHUD.SetActive(false);

        // Ulozi Highscore pokud je vetsi nez predesle
        int bestScore = PlayerPrefs.GetInt("ShootingHighScore", 0);
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("ShootingHighScore", currentScore);
            PlayerPrefs.Save();
        }

        // Ukoncime interakci, coz vrati chod hry do normalu
        FindObjectOfType<Interactor>().SendMessage("EndInteraction");

        // Tady pak mùžeme pøidat i odmìnu za støelbu (+5% pøesnost), jak máš v konceptu
    }
}