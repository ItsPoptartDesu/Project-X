using System.Collections.Generic;
using UnityEngine;

public class Water_Spear : CardDisplay
{
    public float AccDebuffRate = .5f;
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;
        List<int> hits = NPC_BattleSystem.GenerateNonRepeatingNumbers(0 , _activeTeam.Count-1 , 3);
        foreach (int i in hits)
        {
            Slime hit = _activeTeam[i];
            hit.ApplyDamage(rawCardStats);
            if (Random.value < rawCardStats.GetStatusEffectProbability())
            {
                AccDebuff burn = new AccDebuff(2 , hit , AccDebuffRate);
                hit.ApplyStatusEffect(burn);
            }
        }
        return true;
    }
}

