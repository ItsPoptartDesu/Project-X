using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameEntry : MonoBehaviour
{
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

    private void LoadAssets()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadAssets();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject SlimePrefab;
    public GameObject SpawnSlime()
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
}
