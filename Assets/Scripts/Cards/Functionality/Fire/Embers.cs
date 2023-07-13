using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embers : CardDisplay
{
    public override void OnPlay(List<Slime> _activeTeam)
    {
        List<int> hits = NPC_BattleSystem.GenerateNonRepeatingNumbers(0 , _activeTeam.Count , 3);
        foreach (int i in hits)
        {
            Slime hit = _activeTeam[i];
            hit.ApplyDamage(rawCardStats);
        }
    }
}
