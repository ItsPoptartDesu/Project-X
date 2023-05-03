using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ManaDisplay : MonoBehaviour
{
    private int CurrentMana = 0;
    private int CurrentTurn = 0;
    public const int MaxMana = 10;
    public int GetCurrentMana() { return CurrentMana; }
    public int GetCurrentTurn() { return CurrentTurn; }
    public List<UI_Mana> Mana;
    private const int MANA_LIMIT = 10;
    public void TurnOnMana(int _turn)
    {
        if (_turn > MANA_LIMIT)
            return;
        Mana[_turn].TurnOnMana();
    }
    public void OnPlay(int _cost)
    {
        CurrentMana -= _cost;
        for (int i = 0; i < _cost; i++)
        {
            Mana[i].TurnOffMana();
        }
    }
    public void TurnEnd()
    {
        for (int i = 0; i < Mana.Count; i++)
            Mana[i].TurnOffMana();
    }
    public void UpdateManaDisplay()
    {
        TurnOnMana(CurrentMana);
        CurrentTurn++;
        if(CurrentTurn > MANA_LIMIT)
            CurrentTurn = MANA_LIMIT;
        CurrentMana = CurrentTurn;
    }
}
