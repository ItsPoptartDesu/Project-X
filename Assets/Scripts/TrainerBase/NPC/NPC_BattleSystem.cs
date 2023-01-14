using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum DECK_SLOTS
{
    PLAYER = 0,
    NPC = 1,
    MAX = 2,
    STARTING,
}
public class NPC_BattleSystem : LevelBehavior
{
    [SerializeField]
    private int StartDrawAmount = 5;
    public Transform[] DeckAttachmentPoints = new Transform[2];
    public Transform[] HandAttachmentPoints = new Transform[2];
    private PlayerController user;
    private NPC_Trainer npc;
    [SerializeField]//TODO: make this an array and use DECK_SLOTS enum to index in to them
    private List<SpawnPoints> NPC_SpawnPoints;
    [SerializeField]
    private List<SpawnPoints> Player_SpawnPoints;
    public Canvas mainCanvas;
    private Dictionary<DECK_SLOTS, Queue<SlimeCard>> Decks = new Dictionary<DECK_SLOTS, Queue<SlimeCard>>();
    private Dictionary<DECK_SLOTS, List<SlimeCard>> Hands = new Dictionary<DECK_SLOTS, List<SlimeCard>>();
    private Dictionary<DECK_SLOTS, List<SlimeCard>> Discard = new Dictionary<DECK_SLOTS, List<SlimeCard>>();
    private int[] Mana = new int[2];
    private DECK_SLOTS currentTurn = DECK_SLOTS.STARTING;
    private Queue<SlimeCard> ActionQueue = new Queue<SlimeCard>();

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Update()
    {
        while (ActionQueue.Count > 0)
        {
            SlimeCard card = ActionQueue.Dequeue();
            card.OnPlay();

            card.OnEnterDiscardPile();
            Discard[currentTurn].Add(card);
        }
    }
    public void PreLoadForBattle(PlayerController _player, NPC_Trainer _npc)
    {
        user = _player;
        npc = _npc;
        Debug.Log("PreLoadForBattle");
        Mana[0] = Mana[1] = 0;
        _player.OnBattleStart(Player_SpawnPoints, this);
        _npc.OnBattleStart(NPC_SpawnPoints, this);
        StartCoroutine(Draw(DECK_SLOTS.PLAYER, StartDrawAmount));
        StartCoroutine(Draw(DECK_SLOTS.NPC, StartDrawAmount));
    }
    private IEnumerator Draw(DECK_SLOTS _who, int _numCards)
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        Hands[_who] = new List<SlimeCard>();
        for (int i = 0; i < _numCards; i++)
        {
            SlimeCard deckToHand = Decks[_who].Dequeue();
            deckToHand.AttachParent(HandAttachmentPoints[(int)_who]);
            deckToHand.OnEnterHand();
            if (_who == DECK_SLOTS.NPC)
                deckToHand.ToggleCardBackRoot(true);
            Hands[_who].Add(deckToHand);
            yield return wfs;
        }
    }
    public void CreateDecks(Slime _slime, DECK_SLOTS _who)
    {
        Decks[_who] = new Queue<SlimeCard>();
        Discard[_who] = new List<SlimeCard>();
        Transform who = _who == DECK_SLOTS.PLAYER ? DeckAttachmentPoints[(int)DECK_SLOTS.PLAYER] : DeckAttachmentPoints[(int)DECK_SLOTS.NPC];
        List<SlimeCard> toBeAdded = new List<SlimeCard>();
        foreach (var piece in _slime.GetActiveParts())
        {
            if (piece.GetSlimePart() == ESlimePart.BODY)
                continue;
            SlimeCard Card = ObjectManager.Instance.CreateCard(piece, _who);
            Card.OnEnterDeck();
            toBeAdded.Add(Card);
            Card.AttachParent(who);
        }
        toBeAdded = ShuffleDeck(toBeAdded);
        for (int i = 0; i < toBeAdded.Count; i++)
        {
            Decks[_who].Enqueue(toBeAdded[i]);
        }
    }
    public List<SlimeCard> ShuffleDeck(List<SlimeCard> _toBeShuffled)
    {
        return _toBeShuffled.OrderBy(x => Guid.NewGuid()).ToList();
    }
    //public void ShuffleDecks(DECK_SLOTS _who)
    //{
    //    System.Random rnd = new System.Random();
    //    for (int i = Decks[_who].Count - 1; i >= 0; i--)
    //    {
    //        int index = rnd.Next(i + 1);
    //        var temp = Decks[_who][i];
    //        Decks[_who][index] = temp;
    //    }
    //}
    public override void PostLevelLoad()
    {
        user.DisablePlayerMovementAndRenderer();
        StartCoroutine(SetupBattle());
        mainCanvas.worldCamera = user.GetCamera();
    }
    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(2f);
        currentTurn = DECK_SLOTS.PLAYER;
        PlayerTurn();
        //IncTurn();
    }
    private void PlayerTurn()
    {
    }
    private void IncMana(DECK_SLOTS _who)
    {
        Mana[(int)_who]++;
    }
    private void IncTurn()
    {
        currentTurn++;
        if (currentTurn >= DECK_SLOTS.MAX)
            currentTurn = DECK_SLOTS.PLAYER;
        IncMana(currentTurn);
        PlayerTurn();
    }
    public void AddCardToActionQueue(SlimeCard _card)
    {
        int cost = (int)_card.rawCardStats.GetCost();
        //if the card cost's too much mana
        if (Mana[(int)_card.myOwner] < cost)
        {
            Debug.Log($"Can not play {_card.CardName}: {cost} cost");
            return;
        }
        Debug.Log($"Playing {_card.CardName}");
        _card.myState = CardState.LIMBO;
        Hands[currentTurn].Remove(_card);
        ActionQueue.Enqueue(_card);
    }
}