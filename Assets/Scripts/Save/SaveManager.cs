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

    private JsonSaveData toBeSaved;
    private JsonSaveData saveSlotOne;
    private Dictionary<string, JSONTrainerInfo> TrainerLookup = new Dictionary<string, JSONTrainerInfo>();
    public List<string> TrainersToLoad = new List<string>();
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
    public void SaveTrainerInfo(string _name, JSONTrainerInfo _info)
    {
        if (TrainerLookup.ContainsKey(_name))
            TrainerLookup[_name] = _info;
    }
    public bool LoadTrainers()
    {
        TextAsset file = Resources.Load<TextAsset>("Trainers/Trainers") as TextAsset;
        var trainerInfo = JsonUtility.FromJson<JSONTrainerInfo>(file.text);
        if (trainerInfo == null)
            return false;

        TrainerLookup.Add(trainerInfo.TrainerName, trainerInfo);
        return true;
    }
    public JSONTrainerInfo LookUpTrainer(string _name)
    {
        return TrainerLookup[_name];
    }
    private void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(toBeSaved, true);
        Debug.Log($"Path: {savePath}. Size: {toBeSaved.SavedSlime.Count()}, Context: {jsonString}");
        //using (FileStream filestream = new FileStream(Application.dataPath + "/Resources/" + TrainerFileName, FileMode.Truncate))
        using (FileStream filestream = new FileStream(savePath, FileMode.Truncate)) 
        using (StreamWriter streamwriter = new StreamWriter(filestream))
        {
            streamwriter.Write(jsonString);
            streamwriter.Flush();
        }
    }
    public void UpdateTrainerState(string _who, bool _hasBeenHit = true)
    {
        TrainerLookup[_who].HasBeenBattled = _hasBeenHit;
    }
    public bool GetTrainerState(string _who)
    {
        return TrainerLookup[_who].HasBeenBattled;
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
    public JsonSlimeInfo(Slime _slime)
    {
        myCardType = new List<CardComponentType>();
        foreach (SlimePiece p in _slime.GetActiveParts())
        {
            myCardType.Add(p.GetCardType());
        }
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
    [SerializeField]
    public List<CardComponentType> myCardType;

    public void DebugStatement()
    {
        string statement = SlimeName + " Parts: ";
        foreach (var p in myCardType)
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
    public bool HasBeenBattled;
    public JSONTrainerInfo(string _name)
    {
        teamInfo = new JsonSaveData();
        TrainerName = _name;
    }
    public JsonSaveData teamInfo;
}
[System.Serializable]
public class JSONTrainerList
{
    [SerializeField]
    public List<string> ActiveTrainers;
    public JSONTrainerList()
    {
        ActiveTrainers = new List<string>();
    }
}
