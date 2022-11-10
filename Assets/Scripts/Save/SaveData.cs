using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveData : MonoBehaviour
{
    string FileName = "/GameData.json";
    private string savePath;
    [SerializeField]
    private JsonSaveData toBeSaved;

    public JsonSaveData GetLastSavedGame() { return toBeSaved; }
    public SaveData()
    {
        savePath = Application.persistentDataPath + FileName;
        toBeSaved = new JsonSaveData();
        Debug.Log($"Saving too {savePath}");
    }

    public bool ReadFile()
    {
        if (File.Exists(savePath))
        {
            string fileContent = File.ReadAllText(savePath);
            toBeSaved = JsonUtility.FromJson<JsonSaveData>(fileContent);
            return true;
        }
        return false;
    }

    public void WriteFile()
    {
        List<Slime> tt = GameEntry.Instance.playerInput.GetActiveTeam();
        if (File.Exists(savePath))
            File.Delete(savePath);
        //build the toBeSavedData
        foreach (var t in tt)
        {
            JsonSlimeInfo sInfo = new JsonSlimeInfo();
            foreach (var p in t.GetActiveParts())//get the part names
                sInfo.PartNames.Add(p.GetSlimePartName().ToString());
            //grab the name :TODO NOT ASSIGNED
            sInfo.SlimeName = t.SlimeName;
            //and finally the board pos
            sInfo.TeamPos = t.myBoardPos;
            //finally save the part data to the master list to be written out
            toBeSaved.SavedSlime.Add(sInfo);
        }

        string jsonString = JsonUtility.ToJson(toBeSaved, true);
        Debug.Log($"writting to {savePath} with {jsonString}");
        StreamWriter FileWriter = new StreamWriter(savePath, false);
        FileWriter.Write(jsonString);
        FileWriter.Close();

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
    public void DebugStatement()
    {
        Debug.Log($"{SlimeName} has ");
        foreach (var p in PartNames)
        {
            Debug.Log($"{p} attached to it");
        }
    }
}
