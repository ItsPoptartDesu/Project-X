using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
public enum DECK_SLOTS
{
    PLAYER = 0,
    NPC = 1,
    MAX = 2,
    STARTING,
    GAME_OVER
}
public class Encounter : LevelBehavior
{
    public override void PostLevelLoad()
    {
        Debug.Log($"Encounter::PostLevelLoad() {currentState}");
        isProcessing = false;
        players = new Dictionary<DECK_SLOTS , BaseNPC>();
        players.Add(DECK_SLOTS.PLAYER , ObjectManager.Instance.GetActivePlayer());
        var player = LevelManager.Instance.GetBattleNPC();
        players.Add(DECK_SLOTS.NPC , LevelManager.Instance.GetBattleNPC());
        players[DECK_SLOTS.PLAYER].OnEncounterStart(ManaDisplay[(int)DECK_SLOTS.PLAYER]);
        players[DECK_SLOTS.NPC].OnEncounterStart(ManaDisplay[(int)DECK_SLOTS.NPC]);

        OnEncounterStart(players[DECK_SLOTS.PLAYER].GetActiveTeam() , DECK_SLOTS.PLAYER);
        OnEncounterStart(players[DECK_SLOTS.NPC].GetActiveTeam() , DECK_SLOTS.NPC);

        mainCanvas.worldCamera = ((PlayerController)players[DECK_SLOTS.PLAYER]).GetCamera();
        StartCoroutine(StartGame());
    }
    private void OnEncounterStart(List<Slime> _activeTeam , DECK_SLOTS _owner)
    {
        ObjectType t = _owner == DECK_SLOTS.PLAYER ?
            ObjectType.PlayerEncounterSpawnPos :
            ObjectType.OpponentEncounterSpawnPos;
        UI_NPCBattle UI = (UI_NPCBattle)inGameUIController;
        Transform parent = _owner == DECK_SLOTS.PLAYER ?
            UI.PlayerDeckRoot :
            UI.TrainerDeckRoot;
        Debug.Log($"{_owner} has a team of size {_activeTeam.Count}");
        List<CardDisplay> toBeShuffled = new List<CardDisplay>();
        foreach (Slime s in _activeTeam)
        {
            BoardPos pos = s.GetDNA().TeamPos;
            List<SpawnPoints> sp = GetSpawnPointsByType(t);
            SpawnPoints startPos = sp.Where(x => x.GetSpot() == pos).FirstOrDefault();
            s.AttachParent(startPos.transform);
            s.transform.localScale *= ObjectManager.Instance.BattleScale;
            s.ToggleRenderers();

            startPos.myHealthBar.ToggleHealthBar(true);
            startPos.myHealthBar.SetHealth(s.GetHealth() , s.GetShields());
            s.InitHealthBar(startPos.myHealthBar);

            s.EncounterStartApplyStatusEffects();

            toBeShuffled.AddRange(s.GetActiveParts()
                    .Select(part =>
                    {
                        CardDisplay card = ObjectManager.Instance.CreateCard(part , _owner);
                        card.OnEnterDeck();
                        card.AttachParent(parent);
                        return card;
                    }).ToList());
        }
        players[_owner].BattleBehaviour.Deck = new Queue<CardDisplay>(ShuffleDeck(toBeShuffled));
    }
    private void Draw(int _numCards)
    {
        players[currentState].BattleBehaviour.ManaDisplayReference.OnTurn();
        players[currentState].BattleBehaviour.Hand.Clear();
        UI_NPCBattle UI = (UI_NPCBattle)inGameUIController;
        if (players[currentState].BattleBehaviour.Deck.Count < _numCards)
        {
            List<CardDisplay> ShuffledDiscardPile =
                ShuffleDeck(players[currentState].BattleBehaviour.Discard);
            foreach (CardDisplay card in ShuffledDiscardPile)
                AddCardToDeck(card);
            players[currentState].BattleBehaviour.Discard.Clear();
        }
        Transform parent = currentState == DECK_SLOTS.PLAYER ? UI.Player_Hand_Root : UI.Trainer_Hand_Root;
        for (int i = 0; i < _numCards; i++)
        {
            CardDisplay deckToHand = players[currentState].BattleBehaviour.Deck.Dequeue();
            deckToHand.AttachParent(parent);
            deckToHand.OnEnterHand();
            if (currentState == DECK_SLOTS.NPC)
                deckToHand.ToggleCardBackRoot(true);
            players[currentState].BattleBehaviour.Hand.Add(deckToHand);
        }
    }
    public void AddCardToDeck(CardDisplay _card)
    {
        _card.OnEnterDeck();
        players[currentState].BattleBehaviour.Deck.Enqueue(_card);
    }
    private void NextTurn()
    {
        foreach (CardDisplay card in players[currentState].BattleBehaviour.Hand)
        {
            AddCardToDiscardPile(card);
        }
        players[currentState].BattleBehaviour.Hand.Clear();
        players[currentState].BattleBehaviour.ManaDisplayReference.TurnEnd();
        currentState = currentState == DECK_SLOTS.PLAYER ? DECK_SLOTS.NPC : DECK_SLOTS.PLAYER;
        Draw(DrawAmount);
        StartCoroutine(HandleTimer());
        if (currentState == DECK_SLOTS.NPC)
            StartCoroutine(AITurn());
    }
    private IEnumerator StartGame()
    {
        Draw(DrawAmount);
        StartCoroutine(HandleTimer());
        yield return wfs;
    }
    public void Update()
    {
        CheckActionQueue();
    }
    private IEnumerator End(WIN_STATE _who)
    {
        //pause all game play we found a winner 
        isProcessing = true;
        yield return wfs;
        GameEntry.Instance.LeaveBattle(ObjectManager.Instance.GetActivePlayer().GetPreviousLevel());
    }
    private IEnumerator AITurn()
    {
        NPC_Trainer cachedOpponent = ((NPC_Trainer)players[DECK_SLOTS.NPC]);
        yield return wfs;
        Debug.Log("AI TURN");
        while (!cachedOpponent.BattleBehaviour.OutOfMana())
        {
            CardDisplay toBePlayed = cachedOpponent.BattleBehaviour.MakeDecision();
            AddCardToActionQueue(toBePlayed);
            Debug.Log($"AI wants to play {toBePlayed.rawCardStats.GetSlimePartName()}");

            //toBePlayed.OnPlay(players[DECK_SLOTS.PLAYER].GetActiveTeam());
        }
        CheckActionQueue();
        NextTurn();
        yield return wfs;
    }
    private IEnumerator HandleTimer()
    {
        float turnTimer = turnDuration;
        while (turnTimer > 0.0f && !isPlayerTurnEnded)
        {
            turnTimer -= Time.deltaTime;
            UpdateTurnTimerUI(turnTimer);
            yield return null;
        }

        // Check if the player turn has ended before the timer runs out
        if (!isPlayerTurnEnded)
        {
            NextTurn();
            yield return wfs;
        }
    }
    public void Button_OnClickEndTurn()
    {
        EndPlayerTurnEarly();
        NextTurn();
    }
    // Call this method when the player ends their turn early
    public void EndPlayerTurnEarly()
    {
        isPlayerTurnEnded = true;
        // If the coroutine is currently running, stop it immediately
        StopCoroutine("HandleTimer");
    }
    private void UpdateTurnTimerUI(float _timeRemaining)
    {
        string formattedTime = string.Format("{0}:{1:00}" ,
            Mathf.FloorToInt(_timeRemaining / 60) ,
            Mathf.FloorToInt(_timeRemaining % 60));
        // timerTextComponent.text = formattedTime;
        ((UI_NPCBattle)inGameUIController).Text_TurnTimer.text = formattedTime;
    }
    private void CheckActionQueue()
    {
        if (isProcessing || ActionQueue.Count == 0)
            return;
        isProcessing = true;
        do
        {
            CardDisplay toBePlayed = ActionQueue.Dequeue();
            List<Slime> TeamToBeHit = toBePlayed.myOwner == DECK_SLOTS.PLAYER ?
                players[DECK_SLOTS.NPC].GetActiveTeam() :
                players[DECK_SLOTS.PLAYER].GetActiveTeam();
            TeamToBeHit = TeamToBeHit.Where(x => !x.IsDead()).ToList();
            if (TeamToBeHit.Count <= 0)
                break;
            toBePlayed.OnPlay(TeamToBeHit.OrderBy(x => x.myBoardPos).ToList());
            players[toBePlayed.myOwner].BattleBehaviour.ManaDisplayReference.OnPlay(
                toBePlayed.rawCardStats.GetCost());
            AddCardToDiscardPile(toBePlayed);
        } while (ActionQueue.Count > 0);
        isProcessing = false;
        IsGameOver();
    }
    protected override void AddCardToDiscardPile(CardDisplay _card)
    {
        Debug.Log($"Moving {_card.rawCardStats.GetSlimePartName()} to the discard pile");
        _card.OnEnterDiscardPile();
        players[currentState].BattleBehaviour.Discard.Add(_card);

        ((UI_NPCBattle)inGameUIController).AddCardToDiscardPile(_card ,
            players[currentState].BattleBehaviour.Discard.Count);
    }
    public override void AddCardToActionQueue(CardDisplay _card)
    {
        int cost = (int)_card.rawCardStats.GetCost();
        BaseAI bb = players[currentState].BattleBehaviour;
        //if the card cost's too much mana
        if (bb.GetCurrentMana() < cost)
        {
            Debug.Log($"Can not play {_card.rawCardStats.GetSlimePartName()}: {cost} cost");
            return;
        }
        Debug.Log($"{currentState} is playing {_card.rawCardStats.GetSlimePartName()}");
        _card.myState = CardState.LIMBO;
        bb.Hand.Remove(_card);
        ActionQueue.Enqueue(_card);
    }
    private void IsGameOver()
    {
        BaseNPC npc = players[DECK_SLOTS.NPC];
        BaseNPC user = players[DECK_SLOTS.PLAYER];
        List<Slime> userActiveTeam = user.GetActiveTeam();
        List<Slime> npcActiveTeam = npc.GetActiveTeam();
        int playerDeadCount = userActiveTeam.Count(s => s.IsDead());
        int npcDeadCount = npcActiveTeam.Count(s => s.IsDead());
        if (playerDeadCount == userActiveTeam.Count && npcDeadCount == npcActiveTeam.Count)
        {
            StartCoroutine(End(WIN_STATE.TIE));
            return;
        }
        if (playerDeadCount == userActiveTeam.Count)
        {
            StartCoroutine(End(WIN_STATE.NPC_WIN));
            return;
        }
        if (npcDeadCount == npcActiveTeam.Count)
        {
            StartCoroutine(End(WIN_STATE.PLAYER_WIN));
            return;
        }
    }

    public int DrawAmount = 5;
    public Canvas mainCanvas;
    public UI_ManaDisplay[] ManaDisplay = new UI_ManaDisplay[(int)DECK_SLOTS.MAX];
    public DECK_SLOTS GetCurrentTurn() { return currentState; }
    private float turnDuration = 45f;
    private DECK_SLOTS currentState = DECK_SLOTS.PLAYER;
    private Dictionary<DECK_SLOTS , BaseNPC> players;
    private WaitForSeconds wfs = new WaitForSeconds(0.3f);
    private bool isPlayerTurnEnded = false;
    private bool isProcessing = false;
}
