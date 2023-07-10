using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum CardState
{
    DECK,
    HAND,
    IN_PLAY,
    DISCARD,
    GRAVE_YARD,
    LIMBO,
}

public abstract class CardBase : MonoBehaviour
{
    [HideInInspector]
    public SlimePiece rawCardStats;
    public DECK_SLOTS myOwner = DECK_SLOTS.STARTING;
    public CardState myState = CardState.DECK;
    [SerializeField]
    protected CardHelper cardHelper;
    public virtual void OnDeckCreation()
    {
        cardHelper.ToggleDisplayRoot(false);
        cardHelper.ToggleCardBackRoot(true);
    }

    public abstract void OnEnterGraveYard();
    public abstract void OnExitGraveYard();
    public abstract void OnEnterHand();
    public abstract void OnExitHand();
    public abstract void OnEnterDeck();
    public abstract void OnExitDeck();
    public abstract void OnPlay(List<Slime> _NPCActiveTeam);
    public abstract void OnEnterDiscardPile();
    public abstract void OnExitDiscardPile();

    public virtual void AssignCardValues(SlimePiece _base , DECK_SLOTS _who) { }
    public virtual void AttachParent(Transform _parent)
    {
        transform.SetParent(_parent);
        transform.position = _parent.position;
    }
    public virtual void ToggleCardBackRoot(bool _display)
    {
        cardHelper.ToggleCardBackRoot(_display);
    }
    public virtual void ToggleCardDisplayRoot(bool _display)
    {
        cardHelper.ToggleDisplayRoot(_display);
    }
}
