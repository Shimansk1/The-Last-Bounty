using System.IO;
using UnityEngine;
namespace TransformSaveLoadSystem
{

public class SaveGameManageris
    {
        public static TransformSaveData CurrentSaveData = new TransformSaveData();

        public const string SaveDirectory = "/SaveData/";
        public const string FileName = "SaveGamePlayerTransform.sav";

        public static bool SaveGame()
        {

            var dir = Application.persistentDataPath + SaveDirectory;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string json = JsonUtility.ToJson(CurrentSaveData, true);
            File.WriteAllText(dir + FileName, json);

            GUIUtility.systemCopyBuffer = dir;

            return true;
        }
        public static void LoadGame()
        {
            string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
            TransformSaveData tempData = new TransformSaveData();

            if(File.Exists(fullPath)) 
            {
                string json = File.ReadAllText(fullPath);
                tempData = JsonUtility.FromJson<TransformSaveData>(json);
            }
            else
            {
                Debug.LogError("Save file does not exist");
            }

            CurrentSaveData = tempData;
        }
        public static void DeleteSaveData()
        {
            string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
