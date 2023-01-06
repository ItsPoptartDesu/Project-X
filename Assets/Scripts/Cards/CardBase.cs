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
    GRAVE_YARD,
}

public abstract class CardBase : MonoBehaviour
{
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI CardDescription;
    public TextMeshProUGUI CardAttack;
    public TextMeshProUGUI CardCost;
    public Image img;
    [HideInInspector]
    public SlimePiece rawCardStats;
    public GameObject CardFrontRoot;
    public GameObject CardBackRoot;

    public CardState myState = CardState.DECK;
    public virtual void OnDeckCreation()
    {
        ToggleDisplayRoot(false);
        ToggleCardBackRoot(true);
    }
    public virtual void ToggleDisplayRoot(bool _on) { CardFrontRoot.SetActive(_on); }
    public virtual void ToggleCardBackRoot(bool _on) { CardBackRoot.SetActive(_on); }
    public abstract void OnEnterBattleField();
    public abstract void OnExitBattleField();
    public abstract void OnEnterGraveYard();
    public abstract void OnExitGraveYard();
    public abstract void OnEnterHand();
    public abstract void OnExitHand();
    public abstract void OnAttack();
    public abstract void OnDefend();
    public abstract void OnEnterDeck();
    public abstract void OnExitDeck();
    public abstract void OnEnterBattlePhase();
    public abstract void OnExitBattlePhase();
    public abstract void OnEnterTurn();
    public abstract void OnExitTurn();
    public abstract void OnEnterIdle();
    public abstract void OnExitIdle();
    public virtual void AssignCardValues(SlimePiece _base) { }
    public virtual void AttachParent(Transform _parent)
    {
        transform.SetParent(_parent);
        transform.position = _parent.position;
    }
}
