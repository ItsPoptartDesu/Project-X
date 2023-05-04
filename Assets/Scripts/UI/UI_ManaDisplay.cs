using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ManaDisplay : MonoBehaviour
{
    private int CurrentManaTurnCap = 0;
    private int CurrentMana = 0;
    public const int MaxMana = 10;
    public int GetCurrentMana() { return CurrentMana; }
    public List<UI_Mana> Mana;
    private const int MANA_LIMIT = 10;
    public void TurnOnMana()
    {
        for (int i = 0; i < CurrentManaTurnCap; i++)
            Mana[i].TurnOnMana();
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
    public void OnTurn()
    {
        CurrentManaTurnCap++;
        if (CurrentManaTurnCap >= MANA_LIMIT)
            CurrentManaTurnCap = MANA_LIMIT;
        CurrentMana = CurrentManaTurnCap;
        TurnOnMana();
    }
}
