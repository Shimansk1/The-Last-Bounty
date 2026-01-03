using TransformSaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameTester : MonoBehaviour
{
    public void SaveGame()
    {
        SaveGameManageris.SaveGame();
    }
    public void LoadGame()
    {
        SaveGameManageris.LoadGame();
    }
}
