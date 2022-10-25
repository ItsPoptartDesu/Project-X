using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    string FileName = "/GameData.json";
    private string saveFile;
    private GameData gameData;
    private void Awake()
    {
        saveFile = Application.persistentDataPath + FileName;
        gameData = new GameData();
    }

    public void ReadFile()
    {
        if (File.Exists(saveFile))
        {
            string fileContent = File.ReadAllText(saveFile);
            gameData = JsonUtility.FromJson<GameData>(fileContent);
        }
    }

    private void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFile, jsonString);
    }

}
