using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_Robe : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;
        if(Random.value < rawCardStats.GetStatusEffectProbability())
        {
            Cleanse cleanse = new Cleanse(1 , rawCardStats.GetHost());
            rawCardStats.GetHost().ApplyBuffStatusEffect(cleanse);
        }
        return true;
    }
}
