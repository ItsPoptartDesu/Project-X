using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    public JsonSaveData GetSaveSlotOne() { return gameData.GetLastSave(); }
    public List<Slime> GetActiveTeam() { return gameData.GetActiveTeam(); }
    public void AddSlimeToTeam(Slime _slime)
    {
        gameData.AddSlimeToTeam(_slime);
    }
    public void FirstLoad()
    {
        savePath = Application.persistentDataPath + FileName;
        saveSlotOne = new JsonSaveData();
        toBeSaved = new JsonSaveData();
        Debug.Log($"Saving too {savePath}"); 
        gameData = new GameData();
        if (ReadFile())
        {
            // we have already played the game before
            gameData.Load(saveSlotOne);
            Debug.Log("LOADING A GAME");
        }
        else
        {
            // this is our first load
            Debug.Log("this is our first load");
        }
       
    }

    public void SaveGame()
    {
        WriteFile();
    }
    string FileName = "/GameData.json";
    private string savePath;
    [SerializeField]
    private JsonSaveData toBeSaved;
    [SerializeField]
    private JsonSaveData saveSlotOne;
    public JsonSaveData GetLastSavedGame() { return saveSlotOne; }

    public bool ReadFile()
    {
        if (File.Exists(savePath))
        {
            string fileContent = File.ReadAllText(savePath);
            saveSlotOne = JsonUtility.FromJson<JsonSaveData>(fileContent);
            gameData.SetLastSave(saveSlotOne);
            if (GameEntry.Instance.isDEBUG)
                foreach (var s in saveSlotOne.SavedSlime)
                    s.DebugStatement();
            return true;
        }
        return false;
    }

    public void WriteFile()
    {
        List<Slime> tt = UIManager.Instance.GetUISlimes();
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
        Debug.Log($"Path: {savePath}. Size: {tt.Count()}, Context: {jsonString}");

        FileStream fileStream = new FileStream(savePath, FileMode.Truncate);
        StreamWriter sw = new StreamWriter(fileStream);
        sw.Write(jsonString);
        sw.Flush();
        fileStream.Flush();
        sw.Close();
        fileStream.Close();
        toBeSaved.SavedSlime.Clear();
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
        string statement = SlimeName + " Parts: ";
        foreach (var p in PartNames)
        {
            statement += p + " & ";
        }
        Debug.Log(statement);
    }
}
