using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public GameData()
    {
        ActiveTeam = new List<Slime>();
    }
    private JsonSaveData LastSave;
    public void SetLastSave(JsonSaveData _FromFile)
    {
        LastSave = _FromFile;
    }
    public JsonSaveData GetLastSave() { return LastSave; }
    private List<Slime> ActiveTeam;
    public void SaveSlimes(List<Slime> _activeTeam)
    {
        ActiveTeam.Clear();
        foreach (Slime s in _activeTeam)
            ActiveTeam.Add(s);
    }

    public void Load(SaveData mySaveData)
    {

        SetLastSave(mySaveData.GetLastSavedGame());
    }
}
