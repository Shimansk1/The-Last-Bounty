using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class WesternDoor : MonoBehaviour, IInteractable
{
    [Header("Nastavení dveøí")]
    [Tooltip("O kolik stupòù se dveøe otevøou (mùže být i záporné èíslo pro opaèný smìr)")]
    [SerializeField] private float openAngle = 90f;
    [Tooltip("Jak dlouho trvá otevøení/zavøení")]
    [SerializeField] private float animationDuration = 1.2f;
    [SerializeField] private bool startsOpen = false;

    [Header("Audio")]
    [SerializeField] private AudioClip creakSound;
    [SerializeField] private AudioClip slamSound;

    private AudioSource audioSource;
    private bool isOpen;
    private bool isAnimating;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    // Implementace IInteractable
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    // U dveøí nepotøebujeme myš (inventáø apod.), takže vracíme false
    public bool RequiresCursorLock => false;

    private void Start()
    {
        // Pøidá/najde AudioSource pro zvuky
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D zvuk

        closedRotation = transform.rotation;
        // Spoèítáme rotaci pro otevøené dveøe (toèíme kolem osy Y)
        openRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);

        isOpen = startsOpen;
        if (isOpen)
        {
            transform.rotation = openRotation;
        }
    }

    // Pozor: Zámìrnì používám tvùj pøeklep "interactSuccesful" (s jedním s), 
    // aby to pøesnì sedìlo na tvùj interface a neházelo to chyby!
    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        // Pokud se dveøe zrovna hýbou, ignorujeme další klikání
        if (isAnimating)
        {
            interactSuccesful = false;
            return;
        }

        interactSuccesful = true;
        isOpen = !isOpen;

        // Spustíme animaci otevírání/zavírání
        StartCoroutine(AnimateDoor(isOpen ? openRotation : closedRotation));
    }

    public void EndInteraction()
    {
        // Dveøe po zmáèknutí 'E' nevyžadují žádnou další akci pro ukonèení interakce.
        // Hráè se mùže hned hýbat dál.
    }

    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        isAnimating = true;

        // Pøehrání zvuku vrzání
        if (creakSound != null)
        {
            audioSource.clip = creakSound;
            audioSource.Play();
        }

        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;

        // Plynulý pøechod rotace
        while (timeElapsed < animationDuration)
        {
            // Quaternion.Slerp zajistí hezký plynulý pohyb
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ujistíme se, že dveøe skonèí pøesnì tam, kde mají
        transform.rotation = targetRotation;
        isAnimating = false;

        // Pokud jsme dveøe zavøeli, pøehrajeme zvuk bouchnutí (pokud ho máme)
        if (!isOpen && slamSound != null)
        {
            audioSource.PlayOneShot(slamSound);
        }

        // Zavoláme event, kdyby ho Interactor nebo nìco jiného poslouchalo
        OnInteractionComplete?.Invoke(this);
    }
}