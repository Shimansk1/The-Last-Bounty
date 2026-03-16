using UnityEngine;
using System.Collections.Generic;

public class QuestEnemyCounter : MonoBehaviour
{
    [Header("Nastaveni Questu")]
    [Tooltip("V jake fazi pribehu musi hrac byt, aby tento pocitadlo fungovalo?")]
    public StoryProgress requiredState = StoryProgress.Kapitola1_VyberDul;

    [Tooltip("Faze, na kterou se pribeh posune po vybiti vsech nepratel")]
    public StoryProgress nextState = StoryProgress.Kapitola2_FalesnyHrac;

    [Header("Nepratele")]
    public List<GameObject> enemiesToKill;

    private bool questCompleted = false;

    void Update()
    {
        // Pokud uz jsme quest splnili, nebo nejsme ve spravne fazi pribehu, nic nepocitame
        if (questCompleted || MainStoryManager.Instance == null) return;
        if (MainStoryManager.Instance.currentState != requiredState) return;

        // Projdeme seznam a odstranime ty, co uz jsou smazani (mrtvi)
        enemiesToKill.RemoveAll(item => item == null);

        // Pokud je seznam prazdny, znamena to, ze jsou vsichni po smrti!
        if (enemiesToKill.Count == 0)
        {
            CompleteObjective();
        }
    }

    void CompleteObjective()
    {
        questCompleted = true;
        Debug.Log("Vsechny cile v dole zlikvidovany!");

        // Posuneme pribeh na dalsi kapitolu
        MainStoryManager.Instance.AdvanceStory(nextState);

        // VOLITELNE: Tady muzes prehrat zvuk splneneho ukolu
        // AudioSource.PlayClipAtPoint(questDoneSound, transform.position);
    }
}