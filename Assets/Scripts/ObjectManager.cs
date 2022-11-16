using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerPrefab;


    private Dictionary<string, SO_SlimePart> LookupTable = new Dictionary<string, SO_SlimePart>();

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

    [SerializeField]
    private List<SO_SlimePart> SO_BodyParts = new List<SO_SlimePart>();

    public static ObjectManager Instance { get; private set; }

    private List<GameObject> toBeDeleted = new List<GameObject>();

    public void DeleteMarkedObjects()
    {
        foreach (GameObject o in toBeDeleted)
            Destroy(o);
    }

    public void MarkObjectToBeDeleted(GameObject _go)
    {
        toBeDeleted.Add(_go);
    }

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
    public GameObject SlimePrefab;
    public GameObject GenerateRandomSlime()
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init();
        SO_SlimePart ToBeRendered = SO_ForeheadParts.ElementAt
            (UnityEngine.Random.Range(0, SO_ForeheadParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.FOREHEAD, ToBeRendered);

        ToBeRendered = SO_EyeParts.ElementAt
    (UnityEngine.Random.Range(0, SO_EyeParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.EYES, ToBeRendered);

        ToBeRendered = SO_EarParts.ElementAt
    (UnityEngine.Random.Range(0, SO_EarParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.EARS, ToBeRendered);

        ToBeRendered = SO_MouthParts.ElementAt
    (UnityEngine.Random.Range(0, SO_MouthParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.MOUTH, ToBeRendered);

        ToBeRendered = SO_BackParts.ElementAt
    (UnityEngine.Random.Range(0, SO_BackParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.BACK, ToBeRendered);

        ToBeRendered = SO_TailParts.ElementAt
    (UnityEngine.Random.Range(0, SO_TailParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.TAIL, ToBeRendered);

        ToBeRendered = SO_BodyParts.ElementAt
(UnityEngine.Random.Range(0, SO_BodyParts.Count));
        slimeComp.UpdateSlimePart(ESlimePart.BODY, ToBeRendered);
        MarkObjectToBeDeleted(slimePrefab);
        return slimePrefab;
    }
    public GameObject GenerateSlime(JsonSlimeInfo _copy)
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init();

        foreach (string partName in _copy.PartNames)
        {
            SO_SlimePart part = LookupTable[partName];
            slimeComp.UpdateSlimePart(part.SlimePart, part);
        }

        MarkObjectToBeDeleted(slimePrefab);
        return slimePrefab;
    }
    public void LoadAssets()
    {
        CreateLookUpTable();
    }
    private void CreateLookUpTable()
    {
        foreach (var p in SO_ForeheadParts)
            LookupTable.Add(p.PartName, p);
        foreach (var p in SO_EarParts)
            LookupTable.Add(p.PartName, p);
        foreach (var p in SO_EyeParts)
            LookupTable.Add(p.PartName, p);
        foreach (var p in SO_MouthParts)
            LookupTable.Add(p.PartName, p);
        foreach (var p in SO_TailParts)
            LookupTable.Add(p.PartName, p);
        foreach (var p in SO_BackParts)
            LookupTable.Add(p.PartName, p);
        foreach (var p in SO_BodyParts)
            LookupTable.Add(p.PartName, p);
    }
    public GameObject GeneratePlayer(/*probly need player save data outside of slime*/)
    {
        GameObject player = Instantiate(PlayerPrefab);
        MarkObjectToBeDeleted(player);
        return player;
    }
}
