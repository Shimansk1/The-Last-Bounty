using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog/Node")]
public class DialogNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogText; // Co NPC øíká

    public List<DialogResponse> responses; // Možnosti odpovìdí
}

[System.Serializable]
public struct DialogResponse
{
    public string responseText; // Co odpoví hráè (napø. "Ukaž mi zboží")
    public DialogNode nextNode; // Kam dialog pokraèuje (nebo null, pokud konèí)

    // Tady je trik pro obchodování:
    public bool triggersShop;
    public bool isExit;
}