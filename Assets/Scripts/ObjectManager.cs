using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum ObjectType
{
    Player,
    NPC,
    Slime,
    Collectible
}


public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField]
    private List<GameObject> EnemyPrefabs = new List<GameObject>();

    private PlayerController ActivePlayer;
    public GameObject GetActivePlayerObject() { return ActivePlayer.gameObject; }
    public PlayerController GetActivePlayer() { return ActivePlayer; }

    [Header("Scriptable Objects")]
    [Space(1)]
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

    [Header("Card Prefabs")]
    [Space(1)]
    [SerializeField]
    private List<SlimeCard> GameCardPrefabs = new List<SlimeCard>();
    private Dictionary<CardComponentType, SlimeCard> CardLookup = new Dictionary<CardComponentType, SlimeCard>();
    private Dictionary<CardComponentType, SO_SlimePart> So_Lookup = new Dictionary<CardComponentType, SO_SlimePart>();


    public static ObjectManager Instance { get; private set; }

    private List<GameObject> toBeDeleted = new List<GameObject>();
    [Space(1)]
    [SerializeField]
    private GameObject CardPrefab;
    public float BattleScale = 75f;
    public void DeleteMarkedObjects()
    {
        Debug.Log("Deleting Cache Items");
        foreach (GameObject o in toBeDeleted)
            Destroy(o);
    }

    public void MarkObjectToBeDeleted(GameObject _go)
    {
        toBeDeleted.Add(_go);
    }
    public SlimeCard CreateCard(SlimePiece _base, DECK_SLOTS _who)
    {
        GameObject card = Instantiate(CardPrefab);
        SlimeCard sCard = card.GetComponent<SlimeCard>();
        sCard.AssignCardValues(_base, _who);
        //sCard.rawCardStats = _base;
        MarkObjectToBeDeleted(card);
        return sCard;
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
    public void LoadPlayer()
    {
        GeneratePlayer();
        ActivePlayer.FirstLoad();
    }
    public GameObject SlimePrefab;
    public GameObject GenerateRandomSlime()
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init(null);
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
    public GameObject GenerateSlime(JsonSlimeInfo _copy, bool _overrideDelete = false)
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init(_copy);

        for(int i = 0; i < _copy.myCardType.Count; i++)
        {
            SO_SlimePart sp = So_Lookup[_copy.myCardType[i]];
            slimeComp.UpdateSlimePart(sp.SlimePart, sp);
        }
        if (!_overrideDelete)
            MarkObjectToBeDeleted(slimePrefab);
        return slimePrefab;
    }
    public void LoadAssets()
    {
        CreatePart_LookupTable();
    }
    private void CreatePart_LookupTable()
    {
        foreach (var p in SO_ForeheadParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }
        foreach (var p in SO_EarParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }
        foreach (var p in SO_EyeParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }
        foreach (var p in SO_MouthParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }
        foreach (var p in SO_TailParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }
        foreach (var p in SO_BackParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }
        foreach (var p in SO_BodyParts)
        {
            So_Lookup.Add(p.CardComponentType, p);
        }

        foreach (var p in GameCardPrefabs)
        {
            CardLookup.Add(p.GetCardType(), p);
        }
    }
    public GameObject GeneratePlayer(/*probly need player save data outside of slime*/)
    {
        GameObject player = Instantiate(PlayerPrefab);
        ActivePlayer = player.GetComponent<PlayerController>();
        return player;
    }
}
