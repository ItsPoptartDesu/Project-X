using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    private List<SaveSlotData> SavedSlots;
    string FileName = "/GameData.json";
    private string savePath;
    private string DirectoryPath;
    private SaveSlotData toBeSaved;
    private SaveSlotData ActiveSave;
    private Dictionary<string , JSONTrainerInfo> TrainerLookup = new Dictionary<string , JSONTrainerInfo>();
    public List<string> TrainersToLoad = new List<string>();
    public SaveSlotData GetActiveSaveGame() { return ActiveSave; }
    public SaveSlotData GetSaveSlotOne() { return gameData.GetLastSave(); }
    public List<Slime> GetActiveTeam() { return gameData.GetActiveTeam(); }
    public string DirectoryName = "SlimeAdventure";
    private JsonSerializerSettings JsonSettings;
    public int ActiveSaveSlot = 0;
    public void AddSlimeToTeam(Slime _slime)
    {
        gameData.AddSlimeToTeam(_slime);
    }
    private bool IsFirstLoad = true;
    public void FirstLoad()
    {
        DirectoryPath = Application.persistentDataPath + "/" + DirectoryName;
        ActiveSave = new SaveSlotData();
        toBeSaved = new SaveSlotData();
        SavedSlots = new List<SaveSlotData>();
        StartCoroutine(DirectoryCheck());
        JsonSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() } ,
            Formatting = Formatting.Indented
        };
        //if (ReadFile())
        //{
        //    // we have already played the game before
        //    //gameData.Load(saveSlotOne);
        //    Debug.Log("LOADING A GAME");
        //}
        //else
        //{
        //    if (!File.Exists(savePath))
        //        File.Create(savePath);
        //    Debug.Log("this is our first load");
        //}
        //if (!LoadTrainers())
        //    Debug.LogError("Failed to Load Trainers");

    }
    public void SavePlayerGame(Vector3 _pos)
    {
        toBeSaved.CurrentWorldName = ObjectManager.Instance.GetActivePlayer().LastPlayableLevel;
        toBeSaved.SavedPosition.Add((int)_pos.x);
        toBeSaved.SavedPosition.Add((int)_pos.y);
        toBeSaved.SavedPosition.Add((int)_pos.z);
        foreach (var s in gameData.GetActiveTeam())
        {
            toBeSaved.ActiveTeam.SavedSlime.Add(new JsonSlimeInfo(s));
        }
        //GameObject randomSlime = ObjectManager.Instance.GenerateRandomSlime();
        //JsonSlimeInfo js = new JsonSlimeInfo(randomSlime.GetComponent<Slime>());
        //toBeSaved.ActiveTeam.SavedSlime.Add(js);
        string jsonString = JsonConvert.SerializeObject(toBeSaved , JsonSettings);
        string path = DirectoryPath + ActiveSaveSlot.ToString() + FileName;

        if (!Directory.Exists(DirectoryPath + ActiveSaveSlot.ToString()))
        {
            Debug.LogError($"Directory '{path}' does not exist!");
            return;
        }
        using (FileStream filestream = new FileStream(path , FileMode.Truncate))
        using (StreamWriter streamwriter = new StreamWriter(filestream))
        {
            streamwriter.Write(jsonString);
            streamwriter.Flush();
        }
        Debug.Log($"CurrentWorldName: {ObjectManager.Instance.GetActivePlayer().LastPlayableLevel}. " +
         $"CurrentWorldName: {toBeSaved.CurrentWorldName}," +
         $" path: {path}");
    }
    /// <summary>
    /// Used on the first time you load into the game. 
    /// Creates game directories for saving and loading
    /// </summary>
    /// <returns></returns>
    private IEnumerator DirectoryCheck()
    {
        yield return StartCoroutine(CreateDirectories());
        if (IsFirstLoad)
            yield return StartCoroutine(NewGame(0));
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator CreateDirectories()
    {
        for (int i = 0; i < 3; i++)
        {
            gameData = new GameData();
            string path = DirectoryPath + i.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log($"Creating Directory at {path}");
            }
            else
            {
                IsFirstLoad = false;
                Debug.Log($"Already a Directory at {path}");
            }
        }
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator NewGame(int _slotID)
    {
        SaveSlotData ssd = new SaveSlotData();
        string jsonString = JsonConvert.SerializeObject(ssd , JsonSettings);
        //string jsonString = JsonConvert.SerializeObject(ssd);//JsonUtility.ToJson(ssd, true);
        string path = DirectoryPath + _slotID.ToString() + FileName;
        Debug.Log($"Path: {path}. Size: {ssd.ActiveTeam.SavedSlime.Count()}, Context: {jsonString}");
        //using (FileStream filestream = new FileStream(Application.dataPath + "/Resources/" + TrainerFileName, FileMode.Truncate))
        if (!Directory.Exists(DirectoryPath + "0"))
        {
            Debug.LogError($"Directory '{path}' does not exist!");
            yield break;
        }


        using (FileStream filestream = new FileStream(path , FileMode.Create))
        using (StreamWriter streamwriter = new StreamWriter(filestream))
        {
            streamwriter.Write(jsonString);
            streamwriter.Flush();
        }
        yield return new WaitForEndOfFrame();
    }
    //used to save from mainUI might be removed later but for now thats how we get Slimes
    public void SaveGame(List<Slime> _fromUI)
    {
        //HashSet<string> keys = new HashSet<string>();
        //foreach (var s in toBeSaved.SavedSlime)
        //    keys.Add(s.secret);
        //foreach (var slime in GetActiveTeam())
        //{
        //    if (keys.Contains(slime.secret))
        //        continue;
        //    JsonSlimeInfo info = new JsonSlimeInfo(slime);
        //    toBeSaved.SavedSlime.Add(info);
        //}
        //foreach (var slime in _fromUI)
        //{
        //    if (keys.Contains(slime.secret))
        //        continue;
        //    JsonSlimeInfo info = new JsonSlimeInfo(slime);
        //    toBeSaved.SavedSlime.Add(info);
        //}
        //WriteFile();
    }
    public bool CheckSaveSlotData(int _slotID)
    {
        string path = Application.persistentDataPath + "/" + DirectoryName + _slotID.ToString();
        return false;
    }
    public bool ReadFile()
    {
        //if (File.Exists(savePath))
        //{
        //    string fileContent = File.ReadAllText(savePath);
        //    saveSlotOne = JsonUtility.FromJson<SaveSlotData>(fileContent);
        //    gameData.SetLastSave(saveSlotOne);
        //    if (GameEntry.Instance.isDEBUG)
        //        foreach (var s in saveSlotOne.SavedSlime)
        //            s.DebugStatement();
        //    return true;
        //}
        return false;
    }
    public void SaveTrainerInfo(string _name , JSONTrainerInfo _info)
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

        TrainerLookup.Add(trainerInfo.TrainerName , trainerInfo);
        return true;
    }
    public JSONTrainerInfo LookUpTrainer(string _name)
    {
        return TrainerLookup[_name];
    }
    private void WriteFile()
    {
        //string jsonString = JsonUtility.ToJson(toBeSaved, true);
        //Debug.Log($"Path: {savePath}. Size: {toBeSaved.SavedSlime.Count()}, Context: {jsonString}");
        ////using (FileStream filestream = new FileStream(Application.dataPath + "/Resources/" + TrainerFileName, FileMode.Truncate))
        //using (FileStream filestream = new FileStream(savePath, FileMode.Truncate)) 
        //using (StreamWriter streamwriter = new StreamWriter(filestream))
        //{
        //    streamwriter.Write(jsonString);
        //    streamwriter.Flush();
        //}
    }
    public void UpdateTrainerState(string _who , bool _hasBeenHit = true)
    {
        TrainerLookup[_who].HasBeenBattled = _hasBeenHit;
    }
    public bool GetTrainerState(string _who)
    {
        return TrainerLookup[_who].HasBeenBattled;
    }
}
[System.Serializable]
public class SaveSlotData
{
    public SaveSlotData()
    {
        ActiveTeam = new SlimeTeamInfo();
        CurrentWorldName = LevelTags.NOT_SET;
        SavedPosition = new List<int>();
    }
    [SerializeField]
    public SlimeTeamInfo ActiveTeam;
    [SerializeField]
    public LevelTags CurrentWorldName;
    [SerializeField]
    public List<int> SavedPosition;

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
public class WorldInfo
{
    [SerializeField]
    public List<JSONTrainerInfo> ActiveTrainers;
    public WorldInfo()
    {
        ActiveTrainers = new List<JSONTrainerInfo>();
    }
}
[System.Serializable]
public class JSONTrainerInfo
{
    public string TrainerName;
    public bool HasBeenBattled;
    public JSONTrainerInfo(string _name)
    {
        ActiveTeam = new SlimeTeamInfo();
        TrainerName = _name;
    }
    public SlimeTeamInfo ActiveTeam;
}
[System.Serializable]
public class SlimeTeamInfo
{
    public SlimeTeamInfo()
    {
        SavedSlime = new List<JsonSlimeInfo>();
    }
    [SerializeField]
    public List<JsonSlimeInfo> SavedSlime;
}

