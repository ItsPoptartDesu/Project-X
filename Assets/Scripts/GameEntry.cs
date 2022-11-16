using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;


    public bool isDEBUG = false;
    public PlayerInput playerInput;

    public static event System.Action OnClickCollectionMenu;
    public static event System.Action OnClickReturnToMainMenu;
    public static GameEntry Instance { get; private set; }


    private FSM_System gameloop;

    [SerializeField]
    LevelTags currentLevel = LevelTags.LEVEL_1;

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

        FSM_Collection FCollection = new FSM_Collection();
        FCollection.AddTransition(Transition.To_Idle, StateID.Idle);

        gameloop.AddState(FIdle);
        gameloop.AddState(FCollection);

        //load games assest
        ObjectManager.Instance.LoadAssets();
        //turn off all UI except for base UI on load
        UIManager.Instance.ResetUI();
        //Turn on camera
        CameraManager.Instance.Init();
        //set up game levels
        LevelManager.Instance.Init();
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadAssets();
        saveManager.FirstLoad();
    }

    // Update is called once per frame
    void Update()
    {
        gameloop.CurrentState.Act(playerInput, gameloop);
        gameloop.CurrentState.Reason(playerInput, gameloop);
    }

    public void Button_OnClickToCollectionClick()
    {
        OnClickCollectionMenu?.Invoke();//changes FSM menu
        //switch menu UI
        UIManager.Instance.ShowCollectionUI();
        JsonSaveData jsd = saveManager.GetSaveSlotOne();
        foreach (var s in jsd.SavedSlime)
        {
            GameObject Slime = ObjectManager.Instance.GenerateSlime(s);
            UIManager.Instance.teamSelectionManager.AttachNewMember(Slime.transform);
        }
    }
    public void Button_OnClickToMenuIdle()
    {
        //change the FSM
        OnClickReturnToMainMenu?.Invoke();
        // switch menu UI
        UIManager.Instance.ResetUI();
        ObjectManager.Instance.DeleteMarkedObjects();
    }
    public void Button_OnClickToPlay()
    {
        //remove any objects that need to be
        ObjectManager.Instance.DeleteMarkedObjects();
        //set camera
        CameraManager.Instance.ToGame();
        //TODO: use different scenes, but for now we testing and its small so its built in
        LevelManager.Instance.ToggleLevel(currentLevel);
        //set correct UI
        UIManager.Instance.ToGameUI();
        //spawn the player
        GameObject Player = ObjectManager.Instance.GeneratePlayer();
        CameraManager.Instance.AttachPlayerCamera(Player);
        LevelManager.Instance.MovePlayerToLevelInfo(Player, currentLevel);
    }
}
