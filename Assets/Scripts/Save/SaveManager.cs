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
    private SaveSlotData SavedSlot;
    string GameDataFileName = "/GameData.json";
    private string DirectoryPath;
    private Dictionary<string , TrainerStatus> TrainerLookup = new Dictionary<string , TrainerStatus>();
    public SaveSlotData GetSaveSlotOne() { return activeGameData.GetLastSave(); }
    public List<Slime> GetActiveTeam() { return activeGameData.GetActiveTeam(); }
    public string DirectoryName = "SlimeAdventure";
    private JsonSerializerSettings JsonSettings;
    private bool hasSavedData = false;
    public bool HasSavedData
    {
        get { return hasSavedData; }
        set { hasSavedData = value; }
    }
    public void FirstLoad()
    {
        DirectoryPath = Application.persistentDataPath + "/" + DirectoryName;
        activeGameData = new GameData();
        SavedSlot = new SaveSlotData();
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
        SavedSlot.CurrentWorldName = ObjectManager.Instance.GetActivePlayer().LastPlayableLevel;
        SavedSlot.SavedPosition = new List<int> { (int)_pos.x , (int)_pos.y , (int)_pos.z };

        // Save the active team's slimes
        SavedSlot.ActiveTeam.SavedSlime.Clear();
        foreach (var s in activeGameData.GetActiveTeam())
        {
            SavedSlot.ActiveTeam.SavedSlime.Add(new JsonSlimeInfo(s));
        }
        // Serialize the saved data to JSON
        string jsonString = JsonConvert.SerializeObject(SavedSlot , JsonSettings);

        // Write the JSON string to a file
        string path = DirectoryPath + GameDataFileName;
        if (!Directory.Exists(DirectoryPath))
        {
            Debug.LogError($"Directory '{path}' does not exist!");
            return;
        }
        File.WriteAllText(path , jsonString);

        Debug.Log($"CurrentWorldName: {ObjectManager.Instance.GetActivePlayer().LastPlayableLevel}. " +
                 $"CurrentWorldName: {SavedSlot.CurrentWorldName}," +
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
        yield return StartCoroutine(LoadJsonSlotData());
        if (!LoadTrainers())
            Debug.Log("ERROR LOADING TRAINERS");
    }
    private IEnumerator LoadJsonSlotData()
    {
        CheckSaveSlotData();
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator CreateDirectories()
    {
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"Creating Directory at {DirectoryPath}");
        }
        else
        {
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"Already a Directory at {DirectoryPath}");
        }
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator NewGame()
    {
        Debug.Log("starting a NEW GAME");
        SaveSlotData ssd = new SaveSlotData();
        string jsonString = JsonConvert.SerializeObject(ssd , JsonSettings);
        string path = DirectoryPath + GameDataFileName;
        Debug.Log($"Path: {path}. Size: {ssd.ActiveTeam.SavedSlime.Count()}, Context: {jsonString}");
        if (!Directory.Exists(DirectoryPath))
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
        SavedSlot = ssd;
        yield return new WaitForEndOfFrame();
    }
    public bool CheckSaveSlotData()
    {
        string path = Application.persistentDataPath + "/" + DirectoryName;
        HasSavedData = LoadJsonToSavedSlot(path);
        Debug.Log($"Load Results: {HasSavedData}");
        return HasSavedData;
    }
    //TODO if return false change button UI to new game. else fill out saved game data. whereever this game function is called.
    public bool LoadJsonToSavedSlot(string _path)
    {
        string filePath = _path + GameDataFileName;
        if (!File.Exists(filePath))
        {
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"Path: {_path}");
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
                Debug.LogError($"Failed to deserialize SaveSlotData at {filePath}");
                return false;
            }
            SavedSlot = ssd;
            if (GameEntry.Instance.isDEBUG)
            {
                foreach (var s in SavedSlot.ActiveTeam.SavedSlime)
                {
                    s.DebugStatement();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception caught while deserializing SaveSlotData at {_path}: {ex.Message}");
            return false;
        }
    }
    public bool ConvertSavedDataToGameData()
    {
        activeGameData.TransferData(SavedSlot);
        return true;
    }
    public bool LoadTrainers()
    {
        string subdirectoryPath = "Trainers";
        TrainerStatus[] scriptableObjects = Resources.LoadAll<TrainerStatus>(subdirectoryPath);
        if (scriptableObjects == null || scriptableObjects.Length == 0)
            return false;
        foreach (TrainerStatus scriptableObject in scriptableObjects)
        {
            TrainerLookup.Add(scriptableObject.name , scriptableObject);
        }
        return true;
    }
    public void ResetTrainerSettings()
    {
        foreach (TrainerStatus trainer in TrainerLookup.Values)
        {
            trainer.HasBeenBattled = false;
        }
    }
    public TrainerStatus LookUpTrainer(string _name)
    {
        return TrainerLookup[_name];
    }
    public void UpdateTrainerStatus(string _name)
    {
        TrainerLookup[_name].HasBeenBattled = true;
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