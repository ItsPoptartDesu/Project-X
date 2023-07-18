using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Cloud : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;
        if (Random.value < rawCardStats.GetStatusEffectProbability())
        {
            Cleanse cleanse = new Cleanse(1 , rawCardStats.GetHost());
            rawCardStats.GetHost().ApplyBuffStatusEffect(cleanse);
        }
        return true;
    }
}
