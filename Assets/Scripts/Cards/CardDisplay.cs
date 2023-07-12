using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class CardDisplay : CardBase,
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
        cardHelper.ToggleDisplayRoot(true);
        cardHelper.ToggleCardBackRoot(false);
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
        cardHelper = GetComponent<CardHelper>();
        cardHelper.CardName.text = _base.GetSlimePartName();
        cardHelper.CardDescription.text = "not filled out yet";
        cardHelper.CardAttack.text = _base.GetPower().ToString();
        cardHelper.CardCost.text = _base.GetCost().ToString();
        cardHelper.img.sprite = _base.GetCardArt();
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
        GameObject clickedOn = eventData.pointerPress;
        CardDisplay card = clickedOn.GetComponent<CardDisplay>();
        if (card != null)
        {
            Debug.Log($"Clicked on {card.rawCardStats.GetSlimePartName()}");
        }
        if (!CanPlayCard || myState == CardState.IN_PLAY)
            return;
        Debug.Log("OnPointerClick");
        ((NPC_BattleSystem)LevelManager.Instance.currentLevelBehaviour).AddCardToActionQueue(card);
        myState = CardState.IN_PLAY;
    }
    /// <summary>
    /// Base functionality is to hit the first slime on the team.
    /// </summary>
    /// <param name="_activeTeam"></param>
    public override void OnPlay(List<Slime> _activeTeam)
    {
        _activeTeam[0].ApplyDamage(rawCardStats);
    }

    public override void OnEnterDiscardPile()
    {
        myState = CardState.DISCARD;
        cardHelper.ToggleCardBackRoot(false);
        cardHelper.ToggleDisplayRoot(false);
    }

    public override void OnExitDiscardPile()
    {
    }
    public virtual void DEBUG_Message()
    {
        Debug.Log($"{myOwner} casts {rawCardStats.GetSlimePartName()} wants to deal {rawCardStats.GetPower()}");
    }
}
