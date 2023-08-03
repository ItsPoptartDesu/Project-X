using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseNPC
{
    public UI_ManaDisplay ManaDisplayReference;
    private string UserName = "PeachRings";
    public string GetUsername() { return UserName; }
    public void SetUsername(string _name) { UserName = _name; }
    public Camera GetCamera() { return myCamera; }
    private PlayerMovement playerMovement;

    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private UI_IGController UIIG_Controller;
    public UI_IGController GetUIController() { return UIIG_Controller; }

    private List<Slime> ActiveTeamDuringBattle = new List<Slime>();
    public override List<Slime> GetActiveTeam() { return SaveManager.Instance.GetActiveTeam(); }
    private LevelTags previousLevel;
    private Vector3 previousPosition;
    public LevelTags LastPlayableLevel;
    public LevelTags GetPreviousLevel() { return previousLevel; }
    public Vector3 GetPreviousPosition() { return previousPosition; }
    public void SetPreviousPosition(Vector3 _location) { previousPosition = _location; }
    public void SetPreviousLevel(LevelTags _returnTo)
    {
        previousLevel = _returnTo;
    }
    public void OnBattleStart(NPC_BattleSystem _system)
    {
        ToggleCamera(true);
        ActiveTeamDuringBattle.Clear();
        ActiveTeamDuringBattle = SaveManager.Instance.GetActiveTeam();
        foreach (var slime in ActiveTeamDuringBattle)
        {
            BoardPos pos = slime.GetDNA().TeamPos;
            SpawnPoints sp = _system.GetSpawnPoint(DECK_SLOTS.PLAYER , pos);
            slime.AttachParent(sp.transform);
            slime.transform.localScale *= ObjectManager.Instance.BattleScale;
            slime.ToggleRenderers();
            HealthBar hb = _system.InitHealhBar(
                DECK_SLOTS.PLAYER ,
                pos ,
                new Vector2(slime.GetHealth() , slime.GetShields()));
            slime.InitHealthBar(hb);
            slime.EncounterStartApplyStatusEffects();
            _system.CreateDecks(slime , DECK_SLOTS.PLAYER);
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (LevelManager.Instance.currentLevelBehaviour == null)
                Debug.Log("NO CURRENT LEVEL");
            UIIG_Controller.GetSettingsMenu().ToggleSelf();
            //OnClickOpenMenu();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            var at = GameEntry.Instance.GetActiveTeam();
            var gd = SaveManager.Instance.activeGameData;
            foreach (var slime in at)
            {
                slime.DebugStatement();
            }
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TogglePlayerMovement(true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log($"Previous Level - {previousLevel} || Current Level - {GameEntry.Instance.GetCurrentLevel()}");
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SaveManager.Instance.SavePlayerGame(GetPreviousPosition());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {

        }
    }
    public void AttachToSelf(Transform _toBeAttached)
    {
        _toBeAttached.SetParent(transform);
    }
    
    public void ToggleCamera(bool _isOn)
    {
        myCamera.enabled = _isOn;
    }
    public void TogglePlayerMovement(bool _isOn)
    {
        playerMovement.enabled = _isOn;
        myRigidbody.velocity = Vector3.zero;
    }
    public void FirstLoad()
    {
        myRenderers = GetComponents<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.FirstLoad();
        myRigidbody = GetComponent<Rigidbody2D>();
        BattleBehaviour = new PlayerAI();
        //turn it off until we load a level
        ToggleRenderers(false);
        TogglePlayerMovement(false);
        ToggleCamera(false);
        DontDestroyOnLoad(gameObject);
    }
    public void OnClickOpenMenu()
    {
        LevelManager.Instance.GetCurrentLevelBehavior().ToggleInGameUI();
    }
    public void EnablePlayerMovementRendererCamera()
    {
        TogglePlayerMovement(true);
        ToggleRenderers(true);
        ToggleCamera(true);
    }
    public void DisablePlayerMovementRendererCamera()
    {
        TogglePlayerMovement(false);
        ToggleRenderers(false);
        ToggleCamera(false);
    }
    public void DisablePlayerMovementAndRenderer()
    {
        TogglePlayerMovement(false);
        ToggleRenderers(false);
    }
    public void ToggleSettingsUI()
    {
        UIIG_Controller.GetSettingsMenu().ToggleSelf();
    }
    public void FSM_Encounter(List<JsonSlimeInfo> _team)
    {
        
    }
    public override void OnEncounterStart(UI_ManaDisplay _mana)
    {
        ToggleCamera(true);
        DisablePlayerMovementAndRenderer();
        BattleBehaviour.ManaDisplayReference = _mana;
    }
}
