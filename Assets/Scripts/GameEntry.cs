using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;
    [SerializeField]
    private GameObject MenuIdle_UI;
    [SerializeField]
    private GameObject MenuCollection_UI;
   
    public bool isDEBUG = false;
    public PlayerInput playerInput;

    public static event System.Action OnClickCollectionMenu;
    public static event System.Action OnClickReturnToMainMenu;
    public static GameEntry Instance { get; private set; }


    private FSM_System gameloop;

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
        //FIdle.AddTransition(Transition.To_Play, StateID.Play);
        //FIdle.AddTransition(Transition.To_Settings, StateID.Settings);
        //FSM_Play FPlay = new FSM_Play();
        //FSM_Settings FSettings = new FSM_Settings();

        FSM_Collection FCollection = new FSM_Collection();
        FCollection.AddTransition(Transition.To_Idle, StateID.Idle);

        gameloop.AddState(FIdle);
        gameloop.AddState(FCollection);

        //turn off all UI except for base UI on load
        MenuCollection_UI.SetActive(false);
        MenuIdle_UI.SetActive(true);
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
        gameloop.CurrentState.Act(this, gameloop);
        gameloop.CurrentState.Reason(this, gameloop);
    }

    public void Button_OnClickToCollectionClick()
    {
        OnClickCollectionMenu?.Invoke();//changes FSM menu
        //switch menu UI
        MenuIdle_UI.SetActive(false);
        MenuCollection_UI.SetActive(true);
    }

    public void Button_OnClickToMenuIdle()
    {
        //change the FSM
        OnClickReturnToMainMenu?.Invoke();
        // switch menu UI
        MenuCollection_UI.SetActive(false);
        MenuIdle_UI.SetActive(true);
    }
}
