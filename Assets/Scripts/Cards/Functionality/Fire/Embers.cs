using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embers : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;

        List<int> hits = NPC_BattleSystem.GenerateRepeatingNumbers(0 , _activeTeam.Count - 1 , 3);
        foreach (int i in hits)
        {
            Slime hit = _activeTeam[i];
            hit.ApplyDamage(rawCardStats);
        }

        return true;
    }
}
