using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;

    string FileName = "/GameData.json";
    private string savePath;

    [SerializeField]
    private JsonSaveData toBeSaved;
    [SerializeField]
    private JsonSaveData saveSlotOne;
    [HideInInspector]
    Dictionary<string, JSONTrainerInfo> TrainerLookup = new Dictionary<string, JSONTrainerInfo>();
    public JsonSaveData GetLastSavedGame() { return saveSlotOne; }


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
            if (!File.Exists(savePath))
                File.Create(savePath);
            Debug.Log("this is our first load");
        }
        if (!LoadTrainers())
            Debug.LogError("Failed to Load Trainers");
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
        foreach (var slime in _fromUI)
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

    public bool LoadTrainers()
    {
        TextAsset file = Resources.Load<TextAsset>("Trainers/Trainers") as TextAsset;
        var trainerInfo = JsonUtility.FromJson<JSONTrainerInfo>(file.text);
        if (trainerInfo != null)
        {
            TrainerLookup.Add(trainerInfo.TrainerName, trainerInfo);
            return true;
        }
        else
            return false;
    }

    public JSONTrainerInfo LookUpTrainer(string _name)
    {
        return TrainerLookup[_name];
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

[System.Serializable]
public class JSONTrainerInfo
{
    public string TrainerName;
    public JSONTrainerInfo(string _name)
    {
        teamInfo = new JsonSaveData();
        TrainerName = _name;
    }
    public JsonSaveData teamInfo;
}