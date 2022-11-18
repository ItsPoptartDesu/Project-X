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
    public PlayerController playerInput;

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
        gameloop.CurrentState.Act(playerInput, gameloop);
        gameloop.CurrentState.Reason(playerInput, gameloop);
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
        CameraManager.Instance.AttachPlayerCamera(ObjectManager.Instance.GetActivePlayerObject());
        LevelManager.Instance.MovePlayerToLevelInfo(ObjectManager.Instance.GetActivePlayerObject(), currentLevel);
    }
}
