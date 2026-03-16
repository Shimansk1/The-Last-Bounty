using UnityEngine;

public enum StoryProgress
{
    Kapitola1_JdiZaSerifem,
    Kapitola1_VyberDul,
    Kapitola2_FalesnyHrac,
    Kapitola3_UcenecAHodinky,
    Kapitola4_PravaRukaBoss,
    Kapitola5_PovoleniOdSerifa,
    Kapitola6_ElDiablo,
    HraDokoncena // --- NOVÉ: Finální vítìzný stav! ---
}

public class MainStoryManager : MonoBehaviour
{
    public static MainStoryManager Instance;

    [Header("Aktualni stav pribehu")]
    public StoryProgress currentState = StoryProgress.Kapitola1_JdiZaSerifem;

    [Header("Audio (Efekty)")]
    public AudioClip questAdvanceSound;
    private AudioSource audioSource;

    [Header("Reputace za Hlavni Pribehy")]
    public float reputationBoostBeforeFinale = 100f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void AdvanceStory(StoryProgress nextState)
    {
        currentState = nextState;
        Debug.Log("Pribeh se posunul na: " + currentState.ToString());

        if (questAdvanceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(questAdvanceSound);
        }

        if (currentState == StoryProgress.Kapitola5_PovoleniOdSerifa && ReputationManager.Instance != null)
        {
            foreach (CityName city in System.Enum.GetValues(typeof(CityName)))
            {
                ReputationManager.Instance.AddReputation(city, reputationBoostBeforeFinale);
            }
        }

        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null) mapManager.UpdateQuestUI();
    }

    public string GetQuestName()
    {
        switch (currentState)
        {
            case StoryProgress.Kapitola1_JdiZaSerifem: return "Serifova neduvera";
            case StoryProgress.Kapitola1_VyberDul: return "Serifova neduvera";
            case StoryProgress.Kapitola2_FalesnyHrac: return "Falesny hrac";
            case StoryProgress.Kapitola3_UcenecAHodinky: return "Ztraceny prekladatel";
            case StoryProgress.Kapitola4_PravaRukaBoss: return "Prava ruka";
            case StoryProgress.Kapitola5_PovoleniOdSerifa: return "Povoleni k lovu";
            case StoryProgress.Kapitola6_ElDiablo: return "El Diablo";
            case StoryProgress.HraDokoncena: return "POSLEDNI ODMENA ZISKANA"; // VÍTÌZSTVÍ
            default: return "Neznamy ukol";
        }
    }

    public string GetQuestDescription()
    {
        switch (currentState)
        {
            case StoryProgress.Kapitola1_JdiZaSerifem:
                return "Najdi Serifa v Hlavnim meste a zeptej se ho na lokaci 'El Diabla'.";
            case StoryProgress.Kapitola1_VyberDul:
                return "Serif ti nepomuze. Musis vycistit stary dul od banditu, aby ti zacal verit.";
            case StoryProgress.Kapitola2_FalesnyHrac:
                return "Serif te poslal do Saloonu. Najdi karbanika, ktery donasi gangu, a poraz ho v kostkach.";
            case StoryProgress.Kapitola3_UcenecAHodinky:
                return "Mas zasifrovany dopis. Bez do Prvni osady a najdi stareho ucence. Asi bude chtit laskavost...";
            case StoryProgress.Kapitola4_PravaRukaBoss:
                return "Z dopisu jsi zjistil, ze prava ruka bosse se schovava nedaleko. Najdi ho a vyzvi ho na duel!";
            case StoryProgress.Kapitola5_PovoleniOdSerifa:
                return "Vis, kde je El Diablo! Vrat se za Serifem. Potrebujes u nej ale 100% Reputaci, aby ti rekl, jak se tam dostat.";
            case StoryProgress.Kapitola6_ElDiablo:
                return "Serif ti dal souradnice. Najdi tajny tabor, zabij El Diabla a jeho bandu. Cas na Posledni Odmenu.";
            case StoryProgress.HraDokoncena:
                return "GRATULUJEME!\n\nEl Diablo je mrtev. Ziskal jsi svou posledni odmenu a muzes si konecne koupit svuj ranc. Divoky zapad je zase o neco bezpecnejsi misto.";
            default:
                return "Cesta konci.";
        }
    }
}