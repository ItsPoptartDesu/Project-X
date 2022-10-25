using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public GameData()
    {
        ActiveTeam = new List<Slime>();
    }
    private List<Slime> ActiveTeam;
    public void SaveSlimes(List<Slime> _activeTeam)
    {
        ActiveTeam.Clear();
        foreach (Slime s in _activeTeam)
            ActiveTeam.Add(s);
    }
}
