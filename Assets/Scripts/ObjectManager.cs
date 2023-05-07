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
    Collectible,
    NULL,
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
    private Dictionary<CardComponentType , SlimeCard> CardLookup = new Dictionary<CardComponentType , SlimeCard>();
    private Dictionary<CardComponentType , SO_SlimePart> So_Lookup = new Dictionary<CardComponentType , SO_SlimePart>();
    private Dictionary<ESlimePart , List<SO_SlimePart>> parts;
    private List<List<SO_SlimePart>> partsLists;
    public static ObjectManager Instance { get; private set; }
    [Space(1)]
    [SerializeField]
    private GameObject CardPrefab;
    public float BattleScale = 75f;
    public SlimeCard CreateCard(SlimePiece _base , DECK_SLOTS _who)
    {
        var g = CardLookup[_base.GetCardType()];
        GameObject card = Instantiate(g.gameObject);
        SlimeCard sCard = card.GetComponent<SlimeCard>();
        sCard.AssignCardValues(_base , _who);
        //sCard.rawCardStats = _base;
        //MarkObjectToBeDeleted(card);
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
    private void Start()
    {
        parts = new Dictionary<ESlimePart , List<SO_SlimePart>>
                {
                    { ESlimePart.FOREHEAD, SO_ForeheadParts },
                    { ESlimePart.EYES, SO_EyeParts },
                    { ESlimePart.EARS, SO_EarParts },
                    { ESlimePart.MOUTH, SO_MouthParts },
                    { ESlimePart.BACK, SO_BackParts },
                    { ESlimePart.TAIL, SO_TailParts },
                    { ESlimePart.BODY, SO_BodyParts }
                };
        partsLists = new List<List<SO_SlimePart>>()
                {
                    SO_ForeheadParts,
                    SO_EarParts,
                    SO_EyeParts,
                    SO_MouthParts,
                    SO_TailParts,
                    SO_BackParts,
                    SO_BodyParts
                };
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

        System.Random rnd = new System.Random();
        foreach (var part in parts)
        {
            SO_SlimePart ToBeRendered = part.Value[rnd.Next(0 , part.Value.Count)];
            slimeComp.UpdateSlimePart(part.Key , ToBeRendered);
        }
        return slimePrefab;
    }
    public GameObject GenerateSlime(JsonSlimeInfo _copy)
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init(_copy);

        for (int i = 0; i < _copy.myCardType.Count; i++)
        {
            SO_SlimePart sp = So_Lookup[_copy.myCardType[i]];
            slimeComp.UpdateSlimePart(sp.SlimePart , sp);
        }
        return slimePrefab;
    }
    public void LoadAssets()
    {
        CreatePart_LookupTable();
    }
    private void CreatePart_LookupTable()
    {
        foreach (var parts in partsLists)
        {
            foreach (var p in parts)
            {
                So_Lookup.Add(p.CardComponentType , p);
            }
        }

        foreach (var p in GameCardPrefabs)
        {
            CardLookup.Add(p.GetCardType() , p);
        }
    }
    public GameObject GeneratePlayer()
    {
        GameObject player = Instantiate(PlayerPrefab);
        ActivePlayer = player.GetComponent<PlayerController>();
        return player;
    }
}
