using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;

    public SaveManager GetSaveManager() { return saveManager; }

    public bool isDEBUG = false;

    public static event System.Action OnClickCollectionMenu;
    public static event System.Action OnClickToGame;
    public static event System.Action OnClickUIToMainMenu;
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

        gameloop.AddState(FIdle);
        gameloop.AddState(FCollection);
        gameloop.AddState(FPlay);

        //load games assest
        ObjectManager.Instance.LoadAssets();
        //turn off all UI except for base UI on load
        UIManager.Instance.ResetUI();
        //Turn on camera
        CameraManager.Instance.Init();
        //set up game levels
        LevelManager.Instance.Init();
        //finally load the player
        ObjectManager.Instance.LoadPlayer();
        CameraManager.Instance.AttachPlayerCamera(ObjectManager.Instance.GetActivePlayerObject());

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
        UIManager.Instance.ShowCollectionUI();
        List<Slime> at = saveManager.GetActiveTeam();
        foreach (var s in at)
        {
            GameObject Slime = ObjectManager.Instance.GenerateSlime(s.dna);
            UIManager.Instance.teamSelectionManager.AttachNewMember(Slime.transform);
        }
    }
    public void Button_OnClickToMenuIdle()
    {
        //change the FSM
        OnClickUIToMainMenu?.Invoke();
        // switch menu UI
        UIManager.Instance.ResetUI();
        ObjectManager.Instance.DeleteMarkedObjects();
    }
    public void Button_OnClickToPlay()
    {
        OnClickToGame?.Invoke();
        //remove any objects that need to be
        ObjectManager.Instance.DeleteMarkedObjects();
        //set camera
        CameraManager.Instance.ToGame();
        //TODO: use different scenes, but for now we testing and its small so its built in
        LevelManager.Instance.ToggleLevel(currentLevel);
        //set correct UI
        UIManager.Instance.ToGameUI();
        CameraManager.Instance.AttachPlayerCamera(ObjectManager.Instance.GetActivePlayerObject());
        LevelManager.Instance.MovePlayerToLevelInfo(ObjectManager.Instance.GetActivePlayerObject(), currentLevel);
        ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementAndRenderer();
        InGameUIController.OnClickGameToMainMenu += LeaveLevel;
    }
    private void LeaveLevel()
    {
        ObjectManager.Instance.DeleteMarkedObjects();
        CameraManager.Instance.GameToMainMenu();
        LevelManager.Instance.GetCurrentLevelBehavior().DisableInGameIU();
        LevelManager.Instance.ToggleLevel(currentLevel);
        UIManager.Instance.ResetUI();
        ObjectManager.Instance.GetActivePlayer().DisablePlayerMovementAndRenderer();
        InGameUIController.OnClickGameToMainMenu -= LeaveLevel;
    }

}
