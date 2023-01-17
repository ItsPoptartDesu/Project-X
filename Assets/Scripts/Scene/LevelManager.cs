using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public enum LevelTags
{
    MainMenu,
    LEVEL_1,
    NPC_Battle,
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
    public LevelBehavior currentLevel;
    private NPC_Trainer EnemyTrainer;
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
    public void OnPlayerEnterClean(GameObject _player, LevelTags _lvlTag)
    {
        //LevelInfo lvl = Levels.Where(x => x.LevelTag == _lvlTag).FirstOrDefault();
        currentLevel = GameObject.FindObjectOfType<LevelBehavior>();
        if (currentLevel != null)
        {
            currentLevel.GetPlayerSpawnPoint(_player);
        }
        if (_lvlTag == LevelTags.NPC_Battle)
        {
            var Player = ObjectManager.Instance.GetActivePlayer();
            ((NPC_BattleSystem)currentLevel).PreLoadForBattle(Player, BattleNPC);
        }
        LoadTrainerData(_lvlTag);
    }
    private NPC_Trainer BattleNPC;

    public void StartBattle(NPC_Trainer _npc, PlayerController _player)
    {
        _player.SetPreviousLevel(GameEntry.Instance.GetCurrentLevel(),_player.transform.position);
        BattleNPC = _npc;
        Debug.Log($"{_npc.name} is in a battle with {_player.GetUsername()}");
        _player.TogglePlayerMovement(false);
        GameEntry.Instance.PlayToBattleTransition(_npc, _player);
    }

    private void DisableLevels()
    {
        foreach (var lvl in Levels)
        {
            lvl.Level.gameObject.SetActive(false);
        }
    }

    public void LoadTrainerData(LevelTags _level)
    {
        currentLevel = GameObject.FindObjectOfType<LevelBehavior>();

        foreach (var t in currentLevel.npc_Trainers)
        {
            JSONTrainerInfo tInfo = GameEntry.Instance.GetSaveManager().LookUpTrainer(t.name);
            t.LoadTrainerData(tInfo);
        }
    }
    public LevelBehavior GetCurrentLevelBehavior()
    {
        if (currentLevel == null)
            currentLevel = GameObject.FindObjectOfType<LevelBehavior>();
        return currentLevel;
    }
    public void Load()
    {
        currentLevel.Load();
    }
    public void Load(LevelTags _level)
    {
        var lvl = Levels.Where(x => x.LevelTag == _level).First();
        lvl.Level.Load();
        GetCurrentLevelBehavior();
    }
}