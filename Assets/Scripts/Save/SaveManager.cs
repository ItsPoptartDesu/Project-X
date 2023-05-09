using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    private SaveSlotData[] SavedSlots;
    string GameDataFileName = "/GameData.json";
    private string DirectoryPath;
    private Dictionary<string , JSONTrainerInfo> TrainerLookup = new Dictionary<string , JSONTrainerInfo>();
    public List<string> TrainersToLoad = new List<string>();
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
        SavedSlots = new SaveSlotData[]
        {
            new SaveSlotData(),
            new SaveSlotData(),
            new SaveSlotData()
        };
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
        // Update the current world name and saved position
        SavedSlots[ActiveSaveSlot].CurrentWorldName = ObjectManager.Instance.GetActivePlayer().LastPlayableLevel;
        SavedSlots[ActiveSaveSlot].SavedPosition = new List<int> { (int)_pos.x , (int)_pos.y , (int)_pos.z };

        // Save the active team's slimes
        SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime.Clear();
        foreach (var s in gameData.GetActiveTeam())
        {
            SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime.Add(new JsonSlimeInfo(s));
        }

        // Add a random slime to the active team
        GameObject randomSlime = ObjectManager.Instance.GenerateRandomSlime();
        JsonSlimeInfo js = new JsonSlimeInfo(randomSlime.GetComponent<Slime>());
        SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime.Add(js);
        SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime[0].TeamPos = BoardPos.F1;
        SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime[0].SlimeName = "Stubs";

        // Serialize the saved data to JSON
        string jsonString = JsonConvert.SerializeObject(SavedSlots[ActiveSaveSlot] , JsonSettings);

        // Write the JSON string to a file
        string path = DirectoryPath + ActiveSaveSlot.ToString() + GameDataFileName;
        if (!Directory.Exists(DirectoryPath + ActiveSaveSlot.ToString()))
        {
            Debug.LogError($"Directory '{path}' does not exist!");
            return;
        }
        File.WriteAllText(path , jsonString);

        Debug.Log($"CurrentWorldName: {ObjectManager.Instance.GetActivePlayer().LastPlayableLevel}. " +
                 $"CurrentWorldName: {SavedSlots[ActiveSaveSlot].CurrentWorldName}," +
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
        Debug.Log("starting a NEW GAME");
        SaveSlotData ssd = new SaveSlotData();
        string jsonString = JsonConvert.SerializeObject(ssd , JsonSettings);
        string path = DirectoryPath + _slotID.ToString() + GameDataFileName;
        Debug.Log($"Path: {path}. Size: {ssd.ActiveTeam.SavedSlime.Count()}, Context: {jsonString}");
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
    public bool CheckSaveSlotData(int _slotID)
    {
        string path = Application.persistentDataPath + "/" + DirectoryName + _slotID.ToString();
        bool LoadResult = LoadJsonToSavedSlot(path , _slotID);
        if (LoadResult)
            ActiveSaveSlot = _slotID;
        Debug.Log($"Load Results: {LoadResult} @ SlotID: {_slotID}");
        return LoadResult;
    }
    public bool LoadJsonToSavedSlot(string _path , int _slotID)
    {
        string filePath = _path + GameDataFileName;
        Debug.Log($"Path:{_path}||SlotID:{_slotID}||FilePath:{filePath}");
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Path: {_path} @ Slot: {_slotID} doesn't exist");
            return false;
        }

        try
        {
            string fileContent = File.ReadAllText(filePath);
            Debug.Log($"fileContent: {fileContent}");
            SaveSlotData ssd = JsonConvert.DeserializeObject<SaveSlotData>(fileContent , JsonSettings);

            if (ssd == null)
            {
                Debug.LogError($"Failed to deserialize SaveSlotData at {filePath} @ Slot: {_slotID}");
                return false;
            }

            SavedSlots[_slotID] = ssd;
            gameData.SetLastSave(SavedSlots[_slotID]);

            if (GameEntry.Instance.isDEBUG)
            {
                foreach (var s in SavedSlots[_slotID].ActiveTeam.SavedSlime)
                {
                    s.DebugStatement();
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception caught while deserializing SaveSlotData at {_path} @ Slot: {_slotID}: {ex.Message}");
            return false;
        }
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
    [SerializeField] public SlimeTeamInfo ActiveTeam;
    [SerializeField] public LevelTags CurrentWorldName;
    [SerializeField] public List<int> SavedPosition;
}
[System.Serializable]
public class SlimeTeamInfo
{
    [SerializeField] public List<JsonSlimeInfo> SavedSlime;
    public SlimeTeamInfo()
    {
        SavedSlime = new List<JsonSlimeInfo>();
    }
}
[System.Serializable]
public class JsonSlimeInfo
{
    [SerializeField] public string SlimeName;
    [SerializeField] public string secret;
    [SerializeField] public BoardPos TeamPos;
    [SerializeField] public List<CardComponentType> myCardType;

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
    public JsonSlimeInfo()
    {
        myCardType = new List<CardComponentType>();
        TeamPos = BoardPos.NA; // initialize to default value
    }
    public void DebugStatement()
    {
        string statement = SlimeName + " Parts: ";
        foreach (var p in myCardType)
        {
            statement += p + " & ";
        }
        statement += TeamPos.ToString();
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
    [SerializeField] public string TrainerName;
    [SerializeField] public bool HasBeenBattled;
    [SerializeField] public SlimeTeamInfo ActiveTeam;
    public JSONTrainerInfo(string _name)
    {
        ActiveTeam = new SlimeTeamInfo();
        TrainerName = _name;
    }
}


