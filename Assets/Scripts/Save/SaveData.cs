using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveData : MonoBehaviour
{
    string FileName = "/GameData.json";
    private string savePath;
    private GameData gameData;
    [SerializeField]
    private JsonSaveData toBeSaved;
    public SaveData()
    {
        savePath = Application.persistentDataPath + FileName;
        gameData = new GameData();
        toBeSaved = new JsonSaveData();
        Debug.Log($"Saving too {savePath}");
    }

    public void ReadFile()
    {
        if (File.Exists(savePath))
        {
            string fileContent = File.ReadAllText(savePath);
            gameData = JsonUtility.FromJson<GameData>(fileContent);
        }
    }

    public void WriteFile()
    {
        List<Slime> tt = GameEntry.Instance.playerInput.GetActiveTeam();
        //build the toBeSavedData
        foreach (var t in tt)
        {
            JsonSlimeInfo sInfo = new JsonSlimeInfo();
            foreach (var p in t.GetActiveParts())//get the part names
                sInfo.PartNames.Add(p.SlimePartName);
            //grab the name :TODO NOT ASSIGNED
            sInfo.SlimeName = t.SlimeName;
            //and finally the board pos
            sInfo.TeamPos = t.myBoardPos;
            //finally save the part data to the master list to be written out
            toBeSaved.SavedSlime.Add(sInfo);
        }

        string jsonString = JsonUtility.ToJson(toBeSaved, true);
        Debug.Log($"writting to {savePath} with {jsonString}");
        File.WriteAllText(savePath, jsonString);
    }

}

[System.Serializable]
public class JsonSaveData
{
    public JsonSaveData()
    {
        SavedSlime = new List<JsonSlimeInfo>();
    }
    [SerializeField]
    public List<JsonSlimeInfo> SavedSlime;
}

[System.Serializable]
public class JsonSlimeInfo
{
    [SerializeField]
    public List<string> PartNames;
    public JsonSlimeInfo()
    {
        PartNames = new List<string>();
    }
    [SerializeField]
    public string SlimeName;

    [SerializeField]
    public BoardPos TeamPos;
}
