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
    //used to save from mainUI might be removed later but for now thats how we get Slimes
    public void SaveGame(List<Slime> _fromUI)
    {
        HashSet<string> keys = new HashSet<string>();
        foreach (var s in toBeSaved.SavedSlime)
            keys.Add(s.secret);
        foreach (var slime in GetActiveTeam())
        {
            if (keys.Contains(slime.secret))
                continue;
            JsonSlimeInfo info = new JsonSlimeInfo(slime);
            toBeSaved.SavedSlime.Add(info);
        }
        foreach(var slime in _fromUI)
        {
            if (keys.Contains(slime.secret))
                continue;
            JsonSlimeInfo info = new JsonSlimeInfo(slime);
            toBeSaved.SavedSlime.Add(info);
        }    
        WriteFile();
    }
    public void SaveGame()
    {
        HashSet<string> keys = new HashSet<string>();
        foreach (var s in toBeSaved.SavedSlime)
            keys.Add(s.secret);
        foreach (var slime in GetActiveTeam())
        {
            if (keys.Contains(slime.secret))
                return;
            JsonSlimeInfo info = new JsonSlimeInfo(slime);
            toBeSaved.SavedSlime.Add(info);
        }
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
    private void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(toBeSaved, true);
        Debug.Log($"Path: {savePath}. Size: {toBeSaved.SavedSlime.Count()}, Context: {jsonString}");
        FileStream filestream = new FileStream(savePath, FileMode.Truncate);
        StreamWriter streamwriter = new StreamWriter(filestream);
        streamwriter.Write(jsonString);
        streamwriter.Flush();
        filestream.Flush();
        streamwriter.Close();
        filestream.Close();
    }
    //public void WriteFileUIONLY()//probly should jsut remove tbh
    //{
    //    List<Slime> tt = UIManager.Instance.GetUISlimes();
    //    //build the toBeSavedData
    //    foreach (var t in tt)
    //    {
    //        JsonSlimeInfo sInfo = new JsonSlimeInfo();
    //        foreach (var p in t.GetActiveParts())//get the part names
    //            sInfo.PartNames.Add(p.GetSlimePartName());
    //        //grab the name :TODO NOT ASSIGNED
    //        sInfo.SlimeName = t.SlimeName;
    //        //and finally the board pos
    //        sInfo.TeamPos = t.myBoardPos;
    //        //finally save the part data to the master list to be written out
    //        toBeSaved.SavedSlime.Add(sInfo);
    //    }

    //    string jsonString = JsonUtility.ToJson(toBeSaved, true);
    //    Debug.Log($"Path: {savePath}. Size: {tt.Count()}, Context: {jsonString}");

    //    FileStream fileStream = new FileStream(savePath, FileMode.Truncate);
    //    StreamWriter sw = new StreamWriter(fileStream);
    //    sw.Write(jsonString);
    //    sw.Flush();
    //    fileStream.Flush();
    //    sw.Close();
    //    fileStream.Close();
    //    toBeSaved.SavedSlime.Clear();
    //}
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
    //public override bool Equals(object obj)
    //{
    //    var other = obj as JsonSlimeInfo;
    //}
    [SerializeField]
    public List<string> PartNames;
    public JsonSlimeInfo(Slime _slime)
    {
        PartNames = new List<string>();
        foreach (var p in _slime.GetActiveParts())
            PartNames.Add(p.GetSlimePartName());
        SlimeName = _slime.SlimeName;
        TeamPos = _slime.myBoardPos;
        secret = _slime.secret;
    }
    [SerializeField]
    public string SlimeName;
    [SerializeField]
    public string secret;
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
