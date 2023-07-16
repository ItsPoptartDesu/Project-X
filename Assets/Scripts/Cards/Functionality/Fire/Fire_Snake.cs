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
            Slime target = _activeTeam[i];
            target.ApplyDamage(rawCardStats);

            if ((target.GetStatusEffect() & rawCardStats.GetOnHitStatusEffect()) == StatusEffect.Burn)
                return; 
            if (Random.value < rawCardStats.GetStatusEffectProbability())
            {
                BurnEffect burn = new BurnEffect(-1 , _activeTeam[0] , SlimeStats.BurnDamage);
                _activeTeam[0].ApplyStatusEffect(burn);
            }
        }
    }
}
