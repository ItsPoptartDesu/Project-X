using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;
    [SerializeField]
    private SceneLoader sceneLoader;
    public SaveManager GetSaveManager() { return saveManager; }

    public bool isDEBUG = false;

    public static event System.Action OnClickCollectionMenu;
    public static event System.Action OnClickToGame;
    public static event System.Action OnClickUIToMainMenu;
    public static event System.Action PlayStateToBattleState;
    public static GameEntry Instance { get; private set; }

    private FSM_System gameloop;

    [SerializeField]
    LevelTags currentLevel = LevelTags.LEVEL_1;
    public LevelTags GetCurrentLevel() { return currentLevel; }
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
        DontDestroyOnLoad(gameObject);
    }

    public List<Slime> GetActiveTeam()
    {
        return saveManager.GetActiveTeam();
    }

    private void LoadAssets()
    {
        //build game loop
        gameloop = new FSM_System();
        FSM_Idle FIdle = new FSM_Idle();
        FIdle.AddTransition(Transition.To_Collection, StateID.Collection);
        FIdle.AddTransition(Transition.To_Play, StateID.Play);

        FSM_Collection FCollection = new FSM_Collection();
        FCollection.AddTransition(Transition.To_Idle, StateID.Idle);

        FSM_Play FPlay = new FSM_Play();
        FPlay.AddTransition(Transition.To_Idle, StateID.Idle);
        FPlay.AddTransition(Transition.To_Battle, StateID.Battle);

        FSM_Battle FBattle = new FSM_Battle();
        FBattle.AddTransition(Transition.To_Play, StateID.Play);

        gameloop.AddState(FIdle);
        gameloop.AddState(FCollection);
        gameloop.AddState(FPlay);
        gameloop.AddState(FBattle);

        //load games assest
        ObjectManager.Instance.LoadAssets();
        //turn off all UI except for base UI on load
        MainMenuUI.Instance.ResetUI();
        //Load Level Defaults
        LevelManager.Instance.Load(currentLevel);
        //finally load the player
        ObjectManager.Instance.LoadPlayer();
    }

    public void PlayToBattleTransition(NPC_Trainer nPC_Trainer, PlayerController playerController)
    {
        PlayStateToBattleState?.Invoke();
    }


    // Start is called before the first frame update
    void Start()
    {
        saveManager.FirstLoad();
        LoadAssets();
    }

    // Update is called once per frame
    void Update()
    {
        gameloop.CurrentState.Act(null, gameloop);
        gameloop.CurrentState.Reason(null, gameloop);
    }

    public void Button_OnClickToCollectionClick()
    {
        OnClickCollectionMenu?.Invoke();//changes FSM menu
        //switch menu UI
        MainMenuUI.Instance.ShowCollectionUI();
        List<Slime> at = saveManager.GetActiveTeam();
        foreach (var s in at)
        {
            GameObject Slime = ObjectManager.Instance.GenerateSlime(s.dna);
            MainMenuUI.Instance.teamSelectionManager.AttachNewMember(Slime.transform);
        }
    }
    public void Button_OnClickToMenuIdle()
    {
        //change the FSM
        OnClickUIToMainMenu?.Invoke();
        // switch menu UI
        MainMenuUI.Instance.ResetUI();
        ObjectManager.Instance.DeleteMarkedObjects();
    }
    public void Button_OnClickToPlay()
    {
        OnClickToGame?.Invoke();
        //remove any objects that need to be
        ObjectManager.Instance.DeleteMarkedObjects();
        //set camera
        SceneLoader.OnAsyncLoadFinish += OnAsyncLevelLoadFinish;
        currentLevel = LevelTags.LEVEL_1;
        sceneLoader.StartAsyncLoad(currentLevel);
        //LevelManager.Instance.LoadTrainerData(currentLevel);
        //set correct UI
        //UIManager.Instance.ToGameUI();
        //CameraManager.Instance.AttachPlayerCamera(ObjectManager.Instance.GetActivePlayerObject());
        //LevelManager.Instance.MovePlayerToLevelInfo(ObjectManager.Instance.GetActivePlayerObject(), currentLevel);
        //ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementAndRenderer();
        //InGameUIController.OnClickGameToMainMenu += LeaveLevel;
    }
    public void OnAsyncLevelLoadFinish()
    {
        Debug.Log("OnAsyncLevelLoadFinish");
        LevelManager.Instance.OnPlayerEnterClean(ObjectManager.Instance.GetActivePlayerObject(), currentLevel);
        ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementRendererCamera();
        InGameUIController.OnClickGameToMainMenu += LeaveLevel;
        SceneLoader.OnAsyncLoadFinish -= OnAsyncLevelLoadFinish;
    }
    private void LeaveLevel()
    {
        currentLevel = LevelTags.MainMenu;
        LevelManager.Instance.Load();
        //LevelManager.Instance.GetCurrentLevelBehavior().DisableInGameIU();
        //UIManager.Instance.ResetUI();
        ObjectManager.Instance.GetActivePlayer().DisablePlayerMovementRendererCamera();
        InGameUIController.OnClickGameToMainMenu -= LeaveLevel;
        sceneLoader.StartAsyncLoad(LevelTags.MainMenu);
    }

}
