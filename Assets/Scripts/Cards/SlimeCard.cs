using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCard : CardBase
{

    public override void OnAttack()
    {
    }

    public override void OnDefend()
    {
    }

    public override void OnEnterBattleField()
    {
    }

    public override void OnEnterBattlePhase()
    {
    }

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

    public override void OnEnterIdle()
    {
    }

    public override void OnEnterTurn()
    {
    }

    public override void OnExitBattleField()
    {
    }

    public override void OnExitBattlePhase()
    {
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

    public override void OnExitIdle()
    {
    }

    public override void OnExitTurn()
    {
    }

    public override void AssignCardValues(SlimePiece _base)
    {
        CardName.text = _base.GetSlimePartName();
        CardDescription.text = "not filled out yet";
        CardAttack.text = _base.GetPower().ToString();
        CardCost.text = _base.GetCost().ToString();
        img.sprite = _base.GetSlimeSprite();
    }
}
