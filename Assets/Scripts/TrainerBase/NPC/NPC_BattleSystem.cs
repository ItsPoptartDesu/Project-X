using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum DECK_SLOTS
{
    PLAYER = 0,
    NPC = 1,
    MAX = 2,
    STARTING,
}
public enum WIN_STATE
{
    PLAYER_WIN,
    NPC_WIN,
    TIE,
    NA,
}

public class NPC_BattleSystem : LevelBehavior
{
    [SerializeField]
    private int StartDrawAmount = 5;
    public Transform[] DeckAttachmentPoints = new Transform[2];
    public Transform[] HandAttachmentPoints = new Transform[2];
    private PlayerController user;
    [SerializeField]//TODO: make this an array and use DECK_SLOTS enum to index in to them
    private List<SpawnPoints> NPC_SpawnPoints;
    [SerializeField]
    private List<SpawnPoints> Player_SpawnPoints;
    public Canvas mainCanvas;
    private Dictionary<DECK_SLOTS , Queue<CardDisplay>> Decks = new Dictionary<DECK_SLOTS , Queue<CardDisplay>>();
    private Dictionary<DECK_SLOTS , List<CardDisplay>> Hands = new Dictionary<DECK_SLOTS , List<CardDisplay>>();
    private Dictionary<DECK_SLOTS , List<CardDisplay>> Discard = new Dictionary<DECK_SLOTS , List<CardDisplay>>();
    private DECK_SLOTS currentTurn = DECK_SLOTS.STARTING;
    private Queue<CardDisplay> ActionQueue = new Queue<CardDisplay>();
    public DECK_SLOTS GetCurrentTurn() { return currentTurn; }
    private WIN_STATE state = WIN_STATE.NA;
    public UI_ManaDisplay[] ManaDisplay = new UI_ManaDisplay[2];

    private bool isProcessing = false;
    private WaitForSeconds wfs = new WaitForSeconds(1f);
    private WaitForSeconds endOfTurnPause = new WaitForSeconds(3f);
    public HealthBar InitHealhBar(DECK_SLOTS _who , BoardPos _pos , Vector2 _HealthnShields)
    {
        SpawnPoints sp = GetSpawnPoint(_who , _pos);
        sp.myHealthBar.ToggleHealthBar(true);
        sp.myHealthBar.SetHealth(_HealthnShields);
        return sp.myHealthBar;
    }
    public SpawnPoints GetSpawnPoint(DECK_SLOTS _who , BoardPos _pos)
    {
        return _who == DECK_SLOTS.PLAYER ?
            Player_SpawnPoints.First(x => x.Spot == _pos) :
            NPC_SpawnPoints.First(x => x.Spot == _pos);
    }
    public void Update()
    {
        if (!isProcessing)
            StartCoroutine(ProcessCards());
        if (state != WIN_STATE.NA )
        {
            StartCoroutine(End());
        }
    }

    IEnumerator ProcessCards()
    {
        isProcessing = true;
        NPC_Trainer npc = LevelManager.Instance.GetBattleNPC();

        while (ActionQueue.Count > 0)
        {
            CardDisplay card = ActionQueue.Dequeue();
            //if the palyer is playing a card get the NPC team and vise versa
            List<Slime> TeamToBeHit = currentTurn == DECK_SLOTS.PLAYER ? npc.ActiveTeam : user.GetActiveTeam();
            Debug.Log($"{currentTurn} is playing {card.rawCardStats.GetSlimePartName()}");
            yield return wfs;

            card.OnPlay(TeamToBeHit.OrderBy(x => x.myBoardPos).ToList());
            ManaDisplay[(int)currentTurn].OnPlay(card.rawCardStats.GetCost());
            card.OnEnterDiscardPile();
            AddCardToDiscardPile(card);
            ((UI_NPCBattle)LevelManager.Instance.currentLevelBehaviour.inGameUIController).AddCardToDiscardPile(card , Discard[currentTurn].Count);
            if (currentTurn == DECK_SLOTS.NPC && ActionQueue.Count == 0)
            {
                Debug.Log($"{currentTurn} is ending their turn");
                IncTurn();
            }
            Debug.Log($"UPDATE {currentTurn} || Deck Size {Decks[currentTurn].Count} || Hand Size {Hands[currentTurn].Count} || Discard Size {Discard[currentTurn].Count}");

            state = IsGameOver();
            if (state != WIN_STATE.NA)
                break;
        }
        isProcessing = false;
        yield return endOfTurnPause;
    }

    IEnumerator End()
    {
        //play particles tell player they are leaving
        yield return new WaitForSeconds(1f);
        GameEntry.Instance.LeaveBattle(ObjectManager.Instance.GetActivePlayer().GetPreviousLevel());
    }
    public void PreLoadForBattle(PlayerController _player)
    {
        user = _player;
        Debug.Log("PreLoadForBattle");
        _player.OnBattleStart(this);
        LevelManager.Instance.GetBattleNPC().OnBattleStart(this);
        currentTurn = DECK_SLOTS.PLAYER;
        TurnPicker();
    }
    private void Draw(int _numCards)
    {
        Debug.Log($"BEFORE {currentTurn} || Deck Size {Decks[currentTurn].Count} || Hand Size {Hands[currentTurn].Count} || Discard Size {Discard[currentTurn].Count}");
        Hands[currentTurn].Clear();
        //if we can't draw the number of cards needed for a hand.
        if (Decks[currentTurn].Count < _numCards)
        {
            List<CardDisplay> shuffled = ShuffleDeck(Discard[currentTurn]);
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"{currentTurn} is shuffleing {shuffled.Count} cards back in to the deck.");
            foreach (var card in shuffled)
            {
                AddCardToDeck(card);
            }
            Discard[currentTurn].Clear();
        }
        for (int i = 0; i < _numCards; i++)
        {
            CardDisplay deckToHand = Decks[currentTurn].Dequeue();
            deckToHand.AttachParent(HandAttachmentPoints[(int)currentTurn]);
            deckToHand.OnEnterHand();
            if (currentTurn == DECK_SLOTS.NPC)
                deckToHand.ToggleCardBackRoot(true);
            Hands[currentTurn].Add(deckToHand);
        }
        Debug.Log($"AFTER {currentTurn} || Deck Size {Decks[currentTurn].Count} || Hand Size {Hands[currentTurn].Count} || Discard Size {Discard[currentTurn].Count}");
    }
    public void CreateDecks(Slime _slime , DECK_SLOTS _who)
    {
        var toBeShuffled = _slime.GetActiveParts()
                    .Select(part =>
                    {
                        CardDisplay card = ObjectManager.Instance.CreateCard(part , _who);
                        card.OnEnterDeck();
                        card.AttachParent(_who == DECK_SLOTS.PLAYER ?
                            DeckAttachmentPoints[(int)DECK_SLOTS.PLAYER] :
                            DeckAttachmentPoints[(int)DECK_SLOTS.NPC]);
                        return card;
                    }).ToList();
        List<CardDisplay> l = ShuffleDeck(toBeShuffled);
        Decks[_who] = new Queue<CardDisplay>(toBeShuffled);
        Hands[_who] = new List<CardDisplay>();
        Discard[_who] = new List<CardDisplay>();
    }
    public List<CardDisplay> ShuffleDeck(List<CardDisplay> _toBeShuffled)
    {
        List<CardDisplay> shuffled = new List<CardDisplay>(_toBeShuffled);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            //int k = rnd.Next(i + 1);
            int k = Random.Range(0 , i + 1);
            CardDisplay value = shuffled[k];
            shuffled[k] = shuffled[i];
            shuffled[i] = value;
        }
        return shuffled;
    }
    public override void PostLevelLoad()
    {
        user.DisablePlayerMovementAndRenderer();
        mainCanvas.worldCamera = user.GetCamera();
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
        ((UI_NPCBattle)LevelManager.Instance.currentLevelBehaviour.inGameUIController).UpdateTurnDisplay(currentTurn);
    }
    // TODO: play effects when this goes off
    private IEnumerator CheckActiveTeamStatusEffects()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        List<Slime> activeTeam = currentTurn == DECK_SLOTS.PLAYER ?
            user.GetActiveTeam() :
            LevelManager.Instance.GetBattleNPC().ActiveTeam;

        foreach (var slime in activeTeam)
        {
            slime.UpdateStatusEffects();
            yield return wfs;
        }
    }
    private void PlayerTurn()
    {
        ManaDisplay[(int)DECK_SLOTS.PLAYER].OnTurn();
        Draw(StartDrawAmount);
        StartCoroutine(CheckActiveTeamStatusEffects());
    }
    private void NPCTurn()
    {
        ManaDisplay[(int)DECK_SLOTS.NPC].OnTurn();
        Draw(StartDrawAmount);
        StartCoroutine(CheckActiveTeamStatusEffects());
        Debug.Log($"{currentTurn} - NPC Turn");
        List<CardDisplay> cards = Hands[currentTurn];
        CardDisplay CardToPlay = cards.Where(x => x.rawCardStats.GetCost() <= ManaDisplay[(int)currentTurn].GetCurrentMana()).FirstOrDefault();
        AddCardToActionQueue(CardToPlay);
    }
    /// <summary>
    /// OnClick button function for End Turn
    /// </summary>
    public void IncTurn()
    {
        foreach (CardDisplay card in Hands[currentTurn])
        {
            card.OnEnterDiscardPile();
            AddCardToDiscardPile(card);
        }
        Hands[currentTurn].Clear();
        ManaDisplay[(int)currentTurn].TurnEnd();
        currentTurn++;
        if (currentTurn >= DECK_SLOTS.MAX)
            currentTurn = DECK_SLOTS.PLAYER;
        TurnPicker();
    }
    public void AddCardToActionQueue(CardDisplay _card)
    {
        int cost = (int)_card.rawCardStats.GetCost();
        //if the card cost's too much mana
        if (ManaDisplay[(int)currentTurn].GetCurrentMana() < cost)
        {
            Debug.Log($"Can not play {_card.rawCardStats.GetSlimePartName()}: {cost} cost");
            return;
        }
        Debug.Log($"{currentTurn} is playing {_card.rawCardStats.GetSlimePartName()}");
        _card.myState = CardState.LIMBO;
        Hands[currentTurn].Remove(_card);
        ActionQueue.Enqueue(_card);
    }
    private WIN_STATE IsGameOver()
    {
        NPC_Trainer npc = LevelManager.Instance.GetBattleNPC();
        int playerDeadCount = user.GetActiveTeam().Count(s => s.IsDead());
        int npcDeadCount = npc.ActiveTeam.Count(s => s.IsDead());
        if (playerDeadCount == user.GetActiveTeam().Count && npcDeadCount == npc.ActiveTeam.Count())
        {
            return WIN_STATE.TIE;
        }
        if (playerDeadCount == user.GetActiveTeam().Count)
        {
            Debug.Log("NPC Wins");

            return WIN_STATE.NPC_WIN;
        }
        if (npcDeadCount == npc.ActiveTeam.Count)
        {
            Debug.Log("Player Wins");
            return WIN_STATE.PLAYER_WIN;
        }
        else
        {
            return WIN_STATE.NA;
        }
    }
    private void AddCardToDiscardPile(CardDisplay _card)
    {
        Debug.Log($"Moving {_card.rawCardStats.GetSlimePartName()} to the discard pile");
        Discard[currentTurn].Add(_card);
        ((UI_NPCBattle)LevelManager.Instance.currentLevelBehaviour.inGameUIController).AddCardToDiscardPile(_card , Discard[currentTurn].Count);
    }
    private void AddCardToDeck(CardDisplay _card)
    {
        if (GameEntry.Instance.isDEBUG)
            Debug.Log($"{currentTurn} is adding { _card.rawCardStats.GetSlimePartName()} back to his deck pile");
        _card.OnEnterDeck();
        Decks[currentTurn].Enqueue(_card);
    }
    public static List<int> GenerateNonRepeatingNumbers(int minValue , int maxValue , int count)
    {
        if (count > (maxValue - minValue + 1) || count < 0)
        {
            throw new System.ArgumentOutOfRangeException("Invalid count parameter.");
        }

        List<int> numbers = new List<int>(count);
        HashSet<int> generatedNumbers = new HashSet<int>();

        while (numbers.Count < count)
        {
            int number = Random.Range(minValue , maxValue + 1);

            if (generatedNumbers.Add(number))
            {
                numbers.Add(number);
            }
        }

        return numbers;
    }

}