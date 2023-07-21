using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ObjectType
{
    Player,
    NPC,
    Slime,
    Collectible,
    NULL,
}

[System.Serializable]
public struct ImgMatcher
{
    public DeBuffStatusEffect StatusEffect;
    public Sprite Img;
}

public class ObjectManager : MonoBehaviour
{
    public GameObject StatusEffectPrefab;
    public ImgMatcher[] StatusEffectImage;
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
    public CardDisplay CreateCard(SlimePiece _base , DECK_SLOTS _who)
    {
        GameObject card = Instantiate(CardPrefab);
        if(!ComponentMapper.CardComponents.ContainsKey(_base.GetCardType()))
        {
            Debug.LogError($"{_base.GetSlimePartName()} | {_base.GetCardType()} is missing from ComponentMapper CardComponents");
            return null;
        }
        var x = ComponentMapper.CardComponents[_base.GetCardType()];
        card.AddComponent(x);
        CardDisplay sCard = card.GetComponent<CardDisplay>();
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
        foreach (ESlimePart desiredPart in System.Enum.GetValues(typeof(ESlimePart)))
        {
            List<SO_SlimePart> parts = unsortedParts.Where(obj => obj.SlimePart == desiredPart).ToList();

            if (parts.Count > 0)
            {
                SO_SlimePart toBeRendered = parts[Random.Range(0 , parts.Count)];
                slimeComp.UpdateSlimePart(desiredPart , toBeRendered);
            }
            else
                Debug.LogError("HOPE I NEVER SEE THIS ALERT");
        }
        return slimePrefab;
    }
    public GameObject GenerateStatusEffectIcon(DeBuffStatusEffect _ToBeAdded)
    {
        ImgMatcher iconData = StatusEffectImage.Where(x => x.StatusEffect == _ToBeAdded).FirstOrDefault();
        GameObject icon = Instantiate(StatusEffectPrefab);
        icon.GetComponent<UIStatusEffectHelper>().AddStatusIcon(iconData.Img);
        //StatusEffectPrefab.
        return icon;
    }
    public GameObject GenerateSlime(JsonSlimeInfo _copy)
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init(_copy);
        var activeParts = _copy.Genes;
        List<CardComponentType> cardTypes = activeParts
                 .Where(g => g.Allele == GeneAllele.D)
                 .Select(g => g.Part)
                 .ToList();
        for (int i = 0; i < cardTypes.Count; i++)
        {
            if(!So_Lookup.ContainsKey(cardTypes[i]))
            {
                Debug.LogError($"Object Manager::GenerateSlime::Unsorted Parts doesn't have {cardTypes[i]} in its List");
                continue;
            }
            SO_SlimePart sp = So_Lookup[cardTypes[i]];
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
