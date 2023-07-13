using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fire_Snake : CardDisplay
{
    public override void OnPlay(List<Slime> _activeTeam)
    {
        List<int> hits = NPC_BattleSystem.GenerateNonRepeatingNumbers(0 ,
            _activeTeam.Count ,
            2);
        foreach (int i in hits)
        {
            if (Random.value > rawCardStats.GetAccuracy())
                continue;

            Slime hit = _activeTeam[i];
            if (Random.value > rawCardStats.GetStatusEffectProbability())
                hit.ApplyStatusEffect(rawCardStats.GetOnHitStatusEffect());
            hit.ApplyDamage(rawCardStats);
        }
    }
}
