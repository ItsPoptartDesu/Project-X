using System.Collections.Generic;
using UnityEngine;


public class Firewall : CardDisplay
{
    /// <summary>
    /// we want to hit two targets at random next to each other.
    /// </summary>
    /// <param name="_activeTeam"></param>
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;

        bool hit = true;

        if (_activeTeam.Count == 1)
        {
            Helper(_activeTeam[0]);
            return hit;
        }

        int index = Random.Range(1 , _activeTeam.Count - 1);
        int direction = Random.Range(0,2) == 0 ? 1 : -1;

        Helper(_activeTeam[index]);
        index += direction;
        Helper(_activeTeam[index]);

        return hit;
    }

    private void Helper(Slime _target)
    {
        _target.ApplyDamage(rawCardStats);
        if ((_target.GetDebuffStatus() & rawCardStats.GetOnHitStatusEffect()) == DeBuffStatusEffect.Burn)
            return;
        if (Random.value < rawCardStats.GetStatusEffectProbability())
        {
            BurnEffect burn = new BurnEffect(-1 , _target);
            _target.ApplyDebuffStatusEffect(burn);
        }
    }
}

