using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelBehavior : MonoBehaviour
{
    public LevelTags myLevelTag;
    //TODO:probably should do dictionary sorting or use some struct. but system.linq is nice 
    public List<SpawnPoints> spawnPoints = new List<SpawnPoints>();
    public UI_Base inGameUIController;
    public List<NPC_Trainer> npc_Trainers = new List<NPC_Trainer>();
    public Camera lvlCamera;

    public virtual void PostLevelLoad()
    {

    }
    public void GetPlayerSpawnPoint(GameObject _player)
    {
        SpawnPoints sp = spawnPoints.Where(x => x.GetObjectType() == ObjectType.Player).First();
        _player.transform.position = sp.position;
    }
    public List<SpawnPoints> GetSpawnPointsByType(ObjectType _type)
    {
        return spawnPoints.Where(x => x.GetObjectType() == _type).ToList();
    }
    public void MoveToRandomSpawnPoint(GameObject _player)
    {
        System.Random r = new System.Random();
        int Index = r.Next(0, spawnPoints.Count);
        _player.transform.position = spawnPoints[Index].position;
    }

    public void ToggleInGameUI()
    {
        inGameUIController.ToggleSelf();
    }
    public void DisableInGameIU()
    {
        inGameUIController.DisableInGameUI();
    }
    public void Load()
    {
        if (lvlCamera != null)
            lvlCamera.enabled = true;
    }
}
