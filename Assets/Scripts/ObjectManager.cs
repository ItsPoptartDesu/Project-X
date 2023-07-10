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
    private List<SO_SlimePart> unsortedParts = new List<SO_SlimePart>();

    [Header("Card Prefabs")]
    [Space(1)]
    [SerializeField]
    private GameObject CardPrefab;
    private Dictionary<CardComponentType , SO_SlimePart> So_Lookup = new Dictionary<CardComponentType , SO_SlimePart>();
    public static ObjectManager Instance { get; private set; }

    public float BattleScale = 75f;
    public SlimeCard CreateCard(SlimePiece _base , DECK_SLOTS _who)
    {
        GameObject card = Instantiate(CardPrefab);
        var x = ComponentMapper.CardComponents[_base.GetCardType()];
        card.AddComponent(x);
        SlimeCard sCard = card.GetComponent<SlimeCard>();
        sCard.AssignCardValues(_base , _who);
        return sCard;
    }
    private void Awake()
    {
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
        foreach (ESlimePart desiredPart in Enum.GetValues(typeof(ESlimePart)))
        {
            List<SO_SlimePart> parts = unsortedParts.Where(obj => obj.SlimePart == desiredPart).ToList();

            if (parts.Count > 0)
            {
                SO_SlimePart toBeRendered = parts[rnd.Next(0 , parts.Count)];
                slimeComp.UpdateSlimePart(desiredPart , toBeRendered);
            }
            else
                Debug.LogError("HOPE I NEVER SEE THIS ALERT");
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
        foreach (var part in unsortedParts)
            So_Lookup.Add(part.CardComponentType , part);
    }
    public GameObject GeneratePlayer()
    {
        GameObject player = Instantiate(PlayerPrefab);
        ActivePlayer = player.GetComponent<PlayerController>();
        return player;
    }
}
