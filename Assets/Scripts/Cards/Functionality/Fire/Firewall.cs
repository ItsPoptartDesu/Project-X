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
        int index = 0;
        Slime target;

        if(_activeTeam.Count == 1)
        {
            target = _activeTeam[index];
            Helper(target);
            return;
        }

        System.Random rnd = new System.Random();
        index = rnd.Next(1 , _activeTeam.Count - 1);
        
        int direction = rnd.Next(2);
        direction = direction == 0 ? 1 : -1;
        
        target = _activeTeam[index];
        Helper(target);

        index += direction;
        target = _activeTeam[index];
        Helper(target);
    }
    private void Helper(Slime _target)
    {
        _target.ApplyDamage(rawCardStats);
        if ((_target.GetStatusEffect() & rawCardStats.GetOnHitStatusEffect()) == StatusEffect.Burn)
            return;
        if (Random.value < rawCardStats.GetStatusEffectProbability())
        {
            BurnEffect burn = new BurnEffect(-1 , _target , SlimeStats.BurnDamage);
            _target.ApplyStatusEffect(burn);
        }
    }
}

