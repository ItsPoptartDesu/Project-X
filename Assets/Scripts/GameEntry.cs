using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameEntry : MonoBehaviour
{

    public static event System.Action OnClickCollectionMenu;
    public static event System.Action OnClickReturnToMainMenu;

    public static GameEntry Instance { get; private set; }
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
    public bool isDEBUG = false;


    [SerializeField]
    private List<SO_SlimePart> SO_ForeheadParts = new List<SO_SlimePart>();

    [SerializeField]
    private List<SO_SlimePart> SO_EarParts = new List<SO_SlimePart>();

    [SerializeField]
    private List<SO_SlimePart> SO_EyeParts = new List<SO_SlimePart>();

    [SerializeField]
    private List<SO_SlimePart> SO_MouthParts = new List<SO_SlimePart>();

    [SerializeField]
    private List<SO_SlimePart> SO_TailParts = new List<SO_SlimePart>();

    [SerializeField]
    private List<SO_SlimePart> SO_BackParts = new List<SO_SlimePart>();

    private FSM_System gameloop;
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
    }

    // Update is called once per frame
    void Update()
    {
        gameloop.CurrentState.Act(this, gameloop);
        gameloop.CurrentState.Reason(this, gameloop);
    }
    public GameObject SlimePrefab;
    public GameObject GenerateRandomSlime()
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init();

        Sprite ToBeRendered = SO_ForeheadParts.ElementAt
            (UnityEngine.Random.Range(0, SO_ForeheadParts.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.FOREHEAD, ToBeRendered);

        ToBeRendered = SO_EyeParts.ElementAt
    (UnityEngine.Random.Range(0, SO_EyeParts.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.EYES, ToBeRendered);

        ToBeRendered = SO_EarParts.ElementAt
    (UnityEngine.Random.Range(0, SO_EarParts.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.EARS, ToBeRendered);

        ToBeRendered = SO_MouthParts.ElementAt
    (UnityEngine.Random.Range(0, SO_MouthParts.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.MOUTH, ToBeRendered);

        ToBeRendered = SO_BackParts.ElementAt
    (UnityEngine.Random.Range(0, SO_BackParts.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.BACK, ToBeRendered);

        ToBeRendered = SO_TailParts.ElementAt
    (UnityEngine.Random.Range(0, SO_TailParts.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.TAIL, ToBeRendered);
        //toBeSpawned.AddPart(forehead);
        return slimePrefab;
    }

    [SerializeField]
    private GameObject MenuIdle_UI;
    [SerializeField]
    private GameObject MenuCollection_UI;


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
