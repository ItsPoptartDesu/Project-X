using System.Collections.Generic;
using UnityEngine;


public class Firewall : CardDisplay
{
    /// <summary>
    /// we want to hit two targets at random next to each other.
    /// </summary>
    /// <param name="_activeTeam"></param>
    public override void OnPlay(List<Slime> _activeTeam)
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(1 , _activeTeam.Count - 1);
        int direction = rnd.Next(2);
        direction = direction == 0 ? 1 : -1;
        
        Slime target = _activeTeam[index];
        if (Random.value < rawCardStats.GetStatusEffectProbability())
            target.ApplyStatusEffect(StatusEffect.Burn);
        target.ApplyDamage(rawCardStats);
        
        index += direction;
        target = _activeTeam[index];
        if (Random.value < rawCardStats.GetStatusEffectProbability())
            target.ApplyStatusEffect(StatusEffect.Burn);
        target.ApplyDamage(rawCardStats);
    }
}

