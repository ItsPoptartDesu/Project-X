using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Falling_Branch : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;

        _activeTeam[0].ApplyDamage(rawCardStats);
        return true;
    }
}
