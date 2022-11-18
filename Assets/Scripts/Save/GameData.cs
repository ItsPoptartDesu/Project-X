using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public GameData()
    {
        ActiveTeam = new List<Slime>();
    }
    private JsonSaveData LastSave;
    public List<Slime> GetActiveTeam() { return ActiveTeam; }
    public void SetLastSave(JsonSaveData _FromFile)
    {
        LastSave = _FromFile;
    }
    public JsonSaveData GetLastSave() { return LastSave; }
    private List<Slime> ActiveTeam;
    public void AddSlimeToTeam(Slime _slime)
    {
        ActiveTeam.Add(_slime);
    }
    public void SaveSlimes(List<Slime> _activeTeam)
    {
        ActiveTeam.Clear();
        foreach (Slime s in _activeTeam)
            ActiveTeam.Add(s);
    }

    public void Load(JsonSaveData _mySaveData)
    {
        SetLastSave(_mySaveData);
        //convert json to c#
        //foreach(var s in mySaveData.GetLastSavedGame().SavedSlime)
        //{

        //}
    }
}
