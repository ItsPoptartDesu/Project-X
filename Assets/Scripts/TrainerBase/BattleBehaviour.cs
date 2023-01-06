using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleBehaviour : MonoBehaviour
{
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
}
