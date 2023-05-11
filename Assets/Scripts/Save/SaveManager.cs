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
    //TODO: change back to private
    public GameData activeGameData;
    private SaveSlotData[] SavedSlots;
    string GameDataFileName = "/GameData.json";
    string WorldDirectoryName = "/World";
    string WorldDataFileExt = ".json";
    private string DirectoryPath;
    private Dictionary<string , JSONTrainerInfo> TrainerLookup = new Dictionary<string , JSONTrainerInfo>();
    public List<string> TrainersToLoad = new List<string>();
    public SaveSlotData GetSaveSlotOne() { return activeGameData.GetLastSave(); }
    public List<Slime> GetActiveTeam() { return activeGameData.GetActiveTeam(); }
    public string DirectoryName = "SlimeAdventure";
    private JsonSerializerSettings JsonSettings;
    public int ActiveSaveSlot = 0;

    private bool IsFirstLoad = true;
    public void FirstLoad()
    {
        DirectoryPath = Application.persistentDataPath + "/" + DirectoryName;
        activeGameData = new GameData();
        SavedSlots = new SaveSlotData[]
        {
            new SaveSlotData(),
            new SaveSlotData(),
            new SaveSlotData()
        };
        JsonSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() } ,
            Formatting = Formatting.Indented
        };
        StartCoroutine(DirectoryCheck());
    }
    public void SavePlayerGame(Vector3 _pos)
    {
        // Update the current world name and saved position
        SavedSlots[ActiveSaveSlot].CurrentWorldName = ObjectManager.Instance.GetActivePlayer().LastPlayableLevel;
        SavedSlots[ActiveSaveSlot].SavedPosition = new List<int> { (int)_pos.x , (int)_pos.y , (int)_pos.z };

        // Save the active team's slimes
        SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime.Clear();
        foreach (var s in activeGameData.GetActiveTeam())
        {
            SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime.Add(new JsonSlimeInfo(s));
        }

        // Add a random slime to the active team
        //GameObject randomSlime = ObjectManager.Instance.GenerateRandomSlime();
        //JsonSlimeInfo js = new JsonSlimeInfo(randomSlime.GetComponent<Slime>());
        //SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime.Add(js);
        //SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime[0].TeamPos = BoardPos.F1;
        //SavedSlots[ActiveSaveSlot].ActiveTeam.SavedSlime[0].SlimeName = "Stubs";

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
        else
            yield return StartCoroutine(LoadJsonSlotData());

        LoadTrainers();
    }
    private IEnumerator LoadJsonSlotData()
    {
        for (int i = 0; i < 3; i++)
        {
            CheckSaveSlotData(i);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator CreateDirectories()
    {
        for (int i = 0; i < 3; i++)
        {
            string path = DirectoryPath + i.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                if (GameEntry.Instance.isDEBUG)
                    Debug.Log($"Creating Directory at {path}");
            }
            else
            {
                IsFirstLoad = false;
                if (GameEntry.Instance.isDEBUG)
                    Debug.Log($"Already a Directory at {path}");
            }
            string trainerPath = path + WorldDirectoryName;
            if (!Directory.Exists(trainerPath))
                Directory.CreateDirectory(trainerPath);
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
        if (!Directory.Exists(DirectoryPath + _slotID.ToString()))
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
        SavedSlots[_slotID] = ssd;
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
    //TODO if return false change button UI to new game. else fill out saved game data. whereever this game function is called.
    public bool LoadJsonToSavedSlot(string _path , int _slotID)
    {
        string filePath = _path + GameDataFileName;
        if (!File.Exists(filePath))
        {
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"Path: {_path} @ Slot: {_slotID} doesn't exist");
            return false;
        }
        try//had issues logging with the ENUMMEMBER to make it more readable. iono if i still need this but it can't hurt right XD
        {
            string fileContent = File.ReadAllText(filePath);
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"fileContent: {fileContent}");
            SaveSlotData ssd = JsonConvert.DeserializeObject<SaveSlotData>(fileContent , JsonSettings);
            if (ssd == null)
            {
                Debug.LogError($"Failed to deserialize SaveSlotData at {filePath} @ Slot: {_slotID}");
                return false;
            }
            SavedSlots[_slotID] = ssd;
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
    public bool ConvertSavedDataToGameData(int _slotID)
    {
        SaveSlotData ssd = SavedSlots[_slotID];
        if (ssd == null)
            return false;
        ActiveSaveSlot = _slotID;
        activeGameData.TransferData(ssd);
        return true;
    }
    public void SaveWorldToFile()
    {
        //create new world info to be saved out
        WorldInfo worldInfo = new WorldInfo();
        //save out all the trainer who haven't been battled yet
        foreach (var pair in TrainerLookup)
        {
            if (pair.Value.HasBeenBattled)
                continue;
            worldInfo.ActiveTrainers.Add(pair.Key);
        }
        //TODO save out consumeables whenever we do it
        string jsonString = JsonConvert.SerializeObject(worldInfo , JsonSettings);
        string path = DirectoryPath + ActiveSaveSlot.ToString() + WorldDirectoryName + "/" + LevelTags.LEVEL_1.ToString() + WorldDataFileExt;

        using (FileStream filestream = new FileStream(path , FileMode.Truncate))
        using (StreamWriter streamwriter = new StreamWriter(filestream))
        {
            streamwriter.Write(jsonString);
            streamwriter.Flush();
        }
    }
    public void SaveJSONTrainer(string _name)
    {
        JSONTrainerInfo t = new JSONTrainerInfo(_name);
        t.HasBeenBattled = false;

        string jsonString = JsonConvert.SerializeObject(t , JsonSettings);
        string path = DirectoryPath + ActiveSaveSlot.ToString() + WorldDirectoryName + "/" + _name + ".json";
        using (FileStream filestream = new FileStream(path , FileMode.Create))
        using (StreamWriter streamwriter = new StreamWriter(filestream))
        {
            streamwriter.Write(jsonString);
            streamwriter.Flush();
        }
    }
    public bool LoadTrainers()
    {
        string subdirectoryPath = "Trainers"; // Replace with the actual subdirectory path

        // Load all files in the subdirectory
        System.Object[] loadedFiles = Resources.LoadAll(subdirectoryPath);

        // Access the loaded files
        foreach (System.Object file in loadedFiles)
        {
            // Check if the file is a JSON file
            if (file is TextAsset)
            {
                TextAsset loadedJsonFile = (TextAsset)file;
                string jsonString = loadedJsonFile.text;
                // Process the JSON data
                // Example: Deserialize the JSON string into an object
                JSONTrainerInfo dataObject = JsonConvert.DeserializeObject<JSONTrainerInfo>(jsonString , JsonSettings);
                Debug.Log($"Data: {jsonString} || Trainer: {dataObject.TrainerName} || {dataObject.HasBeenBattled} || {dataObject.ActiveTeam.SavedSlime[0].SlimeName} ");
                TrainerLookup.Add(dataObject.TrainerName , dataObject);
            }
        }
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
    public List<string> ActiveTrainers;
    [SerializeField]
    public List<string> Consumables;
    public WorldInfo()
    {
        ActiveTrainers = new List<string>();
        Consumables = new List<string>();
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
        HasBeenBattled = false;
    }
}


