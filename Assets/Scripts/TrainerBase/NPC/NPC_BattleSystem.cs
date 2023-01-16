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
    public DECK_SLOTS GetCurrentTurn() { return currentTurn; }
    public HealthBar InitHealhBar(DECK_SLOTS _who, BoardPos _pos, int _hp)
    {
        SpawnPoints sp = GetSpawnPoint(_who, _pos);
        sp.myHealthBar.ToggleHealthBar(true);
        sp.myHealthBar.SetHealth(_hp);
        return sp.myHealthBar;
    }
    public void UpdateHealthBars(DECK_SLOTS _who, BoardPos _pos, int _hp)
    {
        SpawnPoints sp = GetSpawnPoint(_who, _pos);
        sp.myHealthBar.SetHealth(_hp);
    }
    public SpawnPoints GetSpawnPoint(DECK_SLOTS _who, BoardPos _pos)
    {
        return _who == DECK_SLOTS.PLAYER ?
            Player_SpawnPoints.First(x => x.Spot == _pos) :
            NPC_SpawnPoints.First(x => x.Spot == _pos);
    }
    public void Update()
    {
        while (ActionQueue.Count > 0)
        {
            SlimeCard card = ActionQueue.Dequeue();
            card.OnPlay(npc.ActiveTeam);

            card.OnEnterDiscardPile();
            Discard[currentTurn].Add(card);
            ((UI_NPCBattle)LevelManager.Instance.currentLevel.inGameUIController).AddCardToDiscardPile(card);
        }
    }
    public void PreLoadForBattle(PlayerController _player, NPC_Trainer _npc)
    {
        user = _player;
        npc = _npc;
        Debug.Log("PreLoadForBattle");
        Mana[0] = Mana[1] = 0;
        _player.OnBattleStart(this);
        _npc.OnBattleStart(this);
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
        var l = ShuffleDeck(_slime.GetActiveParts()
                    .Where(part => part.GetSlimePart() != ESlimePart.BODY)
                    .Select(part =>
                    {
                        SlimeCard card = ObjectManager.Instance.CreateCard(part, _who);
                        card.OnEnterDeck();
                        card.AttachParent(_who == DECK_SLOTS.PLAYER ?
                            DeckAttachmentPoints[(int)DECK_SLOTS.PLAYER] :
                            DeckAttachmentPoints[(int)DECK_SLOTS.NPC]);
                        return card;
                    }).ToList());
        Decks[_who] = new Queue<SlimeCard>(l);
        Discard[_who] = new List<SlimeCard>();
    }
    #region Old Create Deck Open AI scares me, just in case XD 
    //Decks[_who] = new Queue<SlimeCard>();
    //Discard[_who] = new List<SlimeCard>();
    //Transform who = _who == DECK_SLOTS.PLAYER ? DeckAttachmentPoints[(int)DECK_SLOTS.PLAYER] : DeckAttachmentPoints[(int)DECK_SLOTS.NPC];
    //List<SlimeCard> toBeAdded = new List<SlimeCard>();
    //foreach (var piece in _slime.GetActiveParts())
    //{
    //    if (piece.GetSlimePart() == ESlimePart.BODY)
    //        continue;
    //    SlimeCard Card = ObjectManager.Instance.CreateCard(piece, _who);
    //    Card.OnEnterDeck();
    //    toBeAdded.Add(Card);
    //    Card.AttachParent(who);
    //}
    //toBeAdded = ShuffleDeck(toBeAdded);
    //for (int i = 0; i < toBeAdded.Count; i++)
    //{
    //    Decks[_who].Enqueue(toBeAdded[i]);
    //}
    #endregion
    public List<SlimeCard> ShuffleDeck(List<SlimeCard> _toBeShuffled)
    {
        System.Random rnd = new System.Random();
        for (int i = _toBeShuffled.Count - 1; i > 0; i--)
        {
            int k = rnd.Next(i + 1);
            SlimeCard value = _toBeShuffled[k];
            _toBeShuffled[k] = _toBeShuffled[i];
            _toBeShuffled[i] = value;
        }
        return _toBeShuffled;
    }
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
        TurnPicker();
        //IncTurn();
    }
    private void TurnPicker()
    {
        switch (currentTurn)
        {
            case DECK_SLOTS.PLAYER:
                PlayerTurn();
                break;
            case DECK_SLOTS.NPC:
                NPCTurn();
                break;
            case DECK_SLOTS.STARTING:
                break;
            default:
                break;
        }
    }
    private void PlayerTurn()
    {

    }
    private void NPCTurn()
    {
        Debug.Log($"{currentTurn} - NPC Turn");
    }
    private void IncMana(DECK_SLOTS _who)
    {
        Mana[(int)_who]++;
    }
    /// <summary>
    /// OnClick button function for End Turn
    /// </summary>
    public void IncTurn()
    {
        foreach(var c in Hands[currentTurn])
        {
            c.OnEnterDiscardPile();
        }
        currentTurn++;
        if (currentTurn >= DECK_SLOTS.MAX)
            currentTurn = DECK_SLOTS.PLAYER;
        IncMana(currentTurn);
        TurnPicker();
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
        Debug.Log($"Playing {_card.CardName.text}");
        _card.myState = CardState.LIMBO;
        Hands[currentTurn].Remove(_card);
        ActionQueue.Enqueue(_card);
    }
}