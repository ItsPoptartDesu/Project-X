using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private SaveData MySaveData;

    private void Awake()
    {

    }

    public void FirstLoad()
    {
        MySaveData = new SaveData();
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
