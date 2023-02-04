using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SlimeCard : CardBase,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool CanDisplayInfo { get { return myOwner == DECK_SLOTS.PLAYER && myState != CardState.DECK; } }
    private bool CanPlayCard { get { return ((NPC_BattleSystem)LevelManager.Instance.currentLevelBehaviour).GetCurrentTurn() == DECK_SLOTS.PLAYER; } }

    public override void OnEnterDeck()
    {
        myState = CardState.DECK;
    }

    public override void OnEnterGraveYard()
    {
    }

    public override void OnEnterHand()
    {
        ToggleDisplayRoot(true);
        ToggleCardBackRoot(false);
        myState = CardState.HAND;
    }

    public override void OnExitDeck()
    {
    }

    public override void OnExitGraveYard()
    {
    }

    public override void OnExitHand()
    {
    }

    public override void AssignCardValues(SlimePiece _base, DECK_SLOTS _who)
    {
        CardName.text = _base.GetSlimePartName();
        CardDescription.text = "not filled out yet";
        CardAttack.text = _base.GetPower().ToString();
        CardCost.text = _base.GetCost().ToString();
        img.sprite = _base.GetSlimeSprite();
        myOwner = _who;
        rawCardStats = _base;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanDisplayInfo)
            return;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!CanDisplayInfo)
            return;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!CanPlayCard)
            return;
        Debug.Log("OnPointerClick");
        GameObject clickedOn = eventData.pointerPress;
        SlimeCard card = clickedOn.GetComponent<SlimeCard>();
        ((NPC_BattleSystem)LevelManager.Instance.currentLevelBehaviour).AddCardToActionQueue(card);
        if (card != null)
        {
            Debug.Log($"Clicked on {card.CardName.text}");
        }
    }
    /// <summary>
    /// Base functionality is to hit the first slime on the team.
    /// </summary>
    /// <param name="_activeTeam"></param>
    public override void OnPlay(List<Slime> _activeTeam)
    {
        myState = CardState.IN_PLAY;
        DEBUG_Message();
        Slime hit = _activeTeam.OrderBy(x => x.stats.dna.TeamPos).First();
        hit.ApplyDamage(rawCardStats);
    }

    public override void OnEnterDiscardPile()
    {
        myState = CardState.DISCARD;
        ToggleCardBackRoot(false);
        ToggleDisplayRoot(false);
    }

    public override void OnExitDiscardPile()
    {
    }
    public virtual void DEBUG_Message()
    {
        Debug.Log($"{myOwner} casts {rawCardStats.GetSlimePartName()} wants to deal {rawCardStats.GetPower()}");
    }
}
