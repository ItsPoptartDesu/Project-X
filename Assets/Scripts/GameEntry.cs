using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;
    [SerializeField]
    private SceneLoader sceneLoader;
    public SaveManager GetSaveManager() { return saveManager; }

    public bool isDEBUG = false;

    public static event System.Action OnClickCollectionMenu;
    public static event System.Action OnClickUIToMainMenu;
    public static event System.Action PlayStateToBattleState;
    public static event System.Action BattleStateToPlayState;
    public static GameEntry Instance { get; private set; }

    private FSM_System gameloop;

    [SerializeField]
    LevelTags currentLevel = LevelTags.LEVEL_1;
    public LevelTags GetCurrentLevel() { return currentLevel; }
    public void SetCurrentLevel(LevelTags _level) { currentLevel = _level; }
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
    /// <summary>
    /// Initial load, loading game assets
    /// </summary>
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
    /// <summary>
    /// Called when the Player wants to battle a NPC Trainer
    /// </summary>
    /// <param name="_npc">trainer the user wants to fight</param>
    /// <param name="_player">local user</param>
    public void PlayToBattleTransition(NPC_Trainer _npc, PlayerController _player)
    {
        //SceneLoader.OnAsyncLoadFinish += OnAsyncLevelLoadFinish;
        sceneLoader.StartAsyncLoad(currentLevel);
        PlayStateToBattleState?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        saveManager.FirstLoad();
        LoadAssets();
    }
    public bool LoadingPause = false;
    // Update is called once per frame
    void Update()
    {
        if (LoadingPause)
            return;
        gameloop.CurrentState.Act(null, gameloop);
        gameloop.CurrentState.Reason(null, gameloop);
    }
    /// <summary>
    /// Main menu to collection scene
    /// </summary>
    public void Button_OnClickToCollectionClick()
    {
        OnClickCollectionMenu?.Invoke();//changes FSM menu
        //switch menu UI
        MainMenuUI.Instance.ShowCollectionUI();
        List<Slime> at = saveManager.GetActiveTeam();
        foreach (var s in at)
        {
            GameObject Slime = ObjectManager.Instance.GenerateSlime(s.dna);//TODO i think i should jsut toggle my preloaded team like battles this is problaby bad
            MainMenuUI.Instance.teamSelectionManager.AttachNewMember(Slime.transform);
        }
    }
    /// <summary>
    /// return to the main menu
    /// </summary>
    public void Button_OnClickToMenuIdle()
    {
        //change the FSM
        OnClickUIToMainMenu?.Invoke();
        // switch menu UI
        MainMenuUI.Instance.ResetUI();
        ObjectManager.Instance.DeleteMarkedObjects();
    }
    /// <summary>
    /// Main Menu to Game Play
    /// </summary>
    public void Button_OnClickToPlay()
    {
        Debug.Log("A - Button_OnClickToPlay");
        GameEntry.Instance.StartLoadLevel(LevelTags.LEVEL_1);
        Debug.Log("B - Button_OnClickToPlay");

        //SceneLoader.OnAsyncLoadFinish += OnAsyncLevelLoadFinish;
    }
    public void StartLoadLevel(LevelTags _level)
    {
        sceneLoader.StartAsyncLoad(_level);
    }
    /// <summary>
    /// Called everytime a new scene finishes loading
    /// </summary>
    public void OnAsyncLevelLoadFinish()
    {
        Debug.Log("OnAsyncLevelLoadFinish");

        //InGameUIController.OnClickGameToMainMenu += LeaveLevel;
        //SceneLoader.OnAsyncLoadFinish -= OnAsyncLevelLoadFinish;
    }
    public void QuitToMainMenu()
    {
        gameloop.PerformTransition(Transition.To_Idle);
    }
    public void LeaveBattle(LevelTags _returnTo)
    {
        currentLevel = _returnTo;
        switch (currentLevel)
        {
            case LevelTags.MainMenu:
            case LevelTags.NPC_Battle:
                ObjectManager.Instance.GetActivePlayer().DisablePlayerMovementRendererCamera();
                break;
            case LevelTags.LEVEL_1:
                ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementRendererCamera();
                LevelManager.Instance.LoadTrainerData(currentLevel);
                break;
            default:
                break;
        }
        sceneLoader.StartAsyncLoad(_returnTo);
        //LevelManager.Instance.Load(currentLevel);
        BattleStateToPlayState?.Invoke();
    }
}
