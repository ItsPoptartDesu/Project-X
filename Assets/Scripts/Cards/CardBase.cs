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
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI CardDescription;
    public TextMeshProUGUI CardAttack;
    public TextMeshProUGUI CardCost;
    public Image img;
    public GameObject CardFrontRoot;
    public GameObject CardBackRoot;
    public DECK_SLOTS myOwner = DECK_SLOTS.STARTING;
    public CardState myState = CardState.DECK;
    [SerializeField]
    private CardComponentType cardType; 
    public CardComponentType GetCardType() { return cardType; }
    public virtual void OnDeckCreation()
    {
        ToggleDisplayRoot(false);
        ToggleCardBackRoot(true);
    }
    public virtual void ToggleDisplayRoot(bool _on) { CardFrontRoot.SetActive(_on); }
    public virtual void ToggleCardBackRoot(bool _on) { CardBackRoot.SetActive(_on); }

    public abstract void OnEnterGraveYard();
    public abstract void OnExitGraveYard();
    public abstract void OnEnterHand();
    public abstract void OnExitHand();
    public abstract void OnEnterDeck();
    public abstract void OnExitDeck();
    public abstract void OnPlay();
    public abstract void OnEnterDiscardPile();
    public abstract void OnExitDiscardPile();

    public virtual void AssignCardValues(SlimePiece _base, DECK_SLOTS _who) { }
    public virtual void AttachParent(Transform _parent)
    {
        transform.SetParent(_parent);
        transform.position = _parent.position;
    }
}
