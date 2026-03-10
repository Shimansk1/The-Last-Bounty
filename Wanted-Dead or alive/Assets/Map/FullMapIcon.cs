using UnityEngine;

public class FullMapIcon : MonoBehaviour
{
    public Transform playerTransform; 
    public RectTransform mapRect;     
    public float worldSize = 3000f;

    void Update()
    {
        // 1. Pøepoèet s opravou zrcadlení (pøidané mínus u osy Z)
        // Pokud jsi vlevo ve svìtì a ikonka je vpravo, musíme Z invertovat
        float xNorm = ((-playerTransform.position.z) / worldSize) + 0.5f;
        float zNorm = (playerTransform.position.x / worldSize) + 0.5f;

        // 2. Aplikace na UI
        float uiX = (xNorm * mapRect.rect.width) - (mapRect.rect.width / 2);
        float uiY = (zNorm * mapRect.rect.height) - (mapRect.rect.height / 2);

        // 3. Rotace šipky (pøizpùsobená pootoèené kameøe)
        float finalRotation = -playerTransform.eulerAngles.y + 90f;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(uiX, uiY);
        transform.localRotation = Quaternion.Euler(0, 0, finalRotation);
    }
}