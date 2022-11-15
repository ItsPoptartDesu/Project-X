using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    private SaveData MySaveData;
    public JsonSaveData GetSaveSlotOne() { return MySaveData.GetLastSavedGame(); }
    private void Awake()
    {

    }

    public void FirstLoad()
    {
        gameData = new GameData();
        MySaveData = new SaveData();
        if (MySaveData.ReadFile())
        {
            // we have already played the game before
            gameData.Load(MySaveData);
            Debug.Log("LOADING A GAME");
        }
        else
        {
            // this is our first load
            Debug.Log("this is our first load");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveGame()
    {
        MySaveData.WriteFile();
    }
}
