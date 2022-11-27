using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public enum LevelTags
{
    LEVEL_1,
}
[System.Serializable]
public struct LevelInfo
{
    public LevelBehavior Level;
    public LevelTags LevelTag;
}
//TODO
//probably could remove the static and put a ref in object manager
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<LevelInfo> Levels = new List<LevelInfo>();
    public void MovePlayerToLevelInfo(GameObject _player, LevelTags _lvlTag)
    {
        LevelInfo lvl = Levels.Where(x => x.LevelTag == _lvlTag).FirstOrDefault();
        if (lvl.Level != null)
        {
            lvl.Level.GetPlayerSpawnPoint(_player);
        }
    }
    public void Init()
    {
        DisableLevels();
    }

    public void StartBattle(NPC_Trainer nPC_Trainer, PlayerController playerController)
    {
        Debug.Log($"{nPC_Trainer.name} is in a battle with {playerController.GetUsername()}");
        playerController.TogglePlayerMovement(false);
        GameEntry.Instance.PlayToBattleTransition(nPC_Trainer, playerController);
    }

    private void DisableLevels()
    {
        foreach (var lvl in Levels)
        {
            lvl.Level.gameObject.SetActive(false);
        }
    }
    public void ToggleLevel(LevelTags _level, bool _isOn)
    {
        LevelInfo lvl = Levels.Where(x => x.LevelTag == _level).FirstOrDefault();
        if (lvl.Level != null)
        {
            lvl.Level.gameObject.SetActive(_isOn);
        }
    }
    public void ToggleLevel(LevelTags _level)
    {
        LevelInfo lvl = Levels.Where(x => x.LevelTag == _level).FirstOrDefault();
        if (lvl.Level != null)
        {
            lvl.Level.gameObject.SetActive(!lvl.Level.gameObject.activeInHierarchy);
        }
    }

    public void LoadTrainerData(LevelTags _level)
    {
        LevelInfo lvl = Levels.Where(x => x.LevelTag == _level).FirstOrDefault();
        foreach (var t in lvl.Level.npc_Trainers)
        {
            JSONTrainerInfo tInfo = GameEntry.Instance.GetSaveManager().LookUpTrainer(t.name);
            t.LoadTrainerData(tInfo);
        }
    }
    public LevelBehavior GetCurrentLevelBehavior()
    {
        return Levels.Where(x => x.LevelTag == GameEntry.Instance.GetCurrentLevel()).FirstOrDefault().Level;
    }
}
