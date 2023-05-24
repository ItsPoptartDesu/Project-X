using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.Serialization;

[System.Serializable]
public enum LevelTags
{
    [EnumMember(Value = "MainMenu")]
    MainMenu,
    [EnumMember(Value = "LEVEL_1")]
    LEVEL_1,
    [EnumMember(Value = "NPC_Battle")]
    NPC_Battle,
    [EnumMember(Value = "NOT_SET")]
    NOT_SET,

}
[System.Serializable]
public struct LevelInfo
{
    public LevelBehavior Level;
    public LevelTags LevelTag;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public LevelBehavior currentLevelBehaviour;
    public LevelTags LoadingLevel;
    private NPC_Trainer BattleNPC; // the trainer the player controller is going to fight, should be null before and after the fight.
    public NPC_Trainer GetBattleNPC() { return BattleNPC; }
    public List<LevelInfo> Levels = new List<LevelInfo>();
    private void Awake()
    {
        // If there is an instance, and it's not me, self immulation
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void OnPlayerEnterClean(GameObject _player, LevelTags _lvlTag)
    {
        currentLevelBehaviour = GameObject.FindObjectOfType<LevelBehavior>();
        if (ObjectManager.Instance.GetActivePlayer().GetPreviousLevel() == LevelTags.NPC_Battle)
        {
            PlayerController pc = _player.GetComponent<PlayerController>();
            _player.transform.SetLocalPositionAndRotation(pc.GetPreviousPosition(), Quaternion.identity);
        }
        else if (currentLevelBehaviour != null)
        {
            currentLevelBehaviour.SetPlayerToSpawnPoint(_player);
        }
        if (_lvlTag == LevelTags.NPC_Battle)
        {
            var Player = ObjectManager.Instance.GetActivePlayer();
            ((NPC_BattleSystem)currentLevelBehaviour).PreLoadForBattle(Player);
        }
    }
    public void StartBattle(NPC_Trainer _npc, PlayerController _player)
    {
        _player.SetPreviousPosition(_player.transform.position);
        _player.SetPreviousLevel(GameEntry.Instance.GetCurrentLevel());
        BattleNPC = _npc;
        Debug.Log($"{_npc.name} is in a battle with {_player.GetUsername()}");
        GameEntry.Instance.PlayToBattleTransition(_npc, _player);
    }
    private void DisableLevels()
    {
        foreach (var lvl in Levels)
        {
            lvl.Level.gameObject.SetActive(false);
        }
    }
    public LevelBehavior GetCurrentLevelBehavior()
    {
        if (currentLevelBehaviour == null)
            currentLevelBehaviour = GameObject.FindObjectOfType<LevelBehavior>();
        return currentLevelBehaviour;
    }
    public void Load()
    {
        currentLevelBehaviour.Load();
    }
    public void Load(LevelTags _level)
    {
        var lvl = Levels.Where(x => x.LevelTag == _level).First();
        lvl.Level.Load();
        GetCurrentLevelBehavior();
    }
}