using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fire_Snake : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        bool hit = false;

        List<int> hits = NPC_BattleSystem.GenerateRepeatingNumbers(0 , _activeTeam.Count-1 , 2);

        foreach (int index in hits)
        {
            if (Random.value <= rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            {
                hit = true;
                Slime target = _activeTeam[index];
                target.ApplyDamage(rawCardStats);

                if ((target.GetDebuffStatus() & rawCardStats.GetOnHitStatusEffect()) != DeBuffStatusEffect.Burn &&
                    Random.value < rawCardStats.GetStatusEffectProbability())
                {
                    BurnEffect burn = new BurnEffect(-1 , target);
                    target.ApplyDebuffStatusEffect(burn);
                }
            }
        }

        return hit;
    }

}
