using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Warm_Winds : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        bool hit = false;
        foreach (Slime s in GameEntry.Instance.GetActiveTeam())
        {
            if (Random.value > s.GetAccuracy(rawCardStats.GetAccuracy()))
                continue;
            if (Random.value > rawCardStats.GetStatusEffectProbability())
                continue;
            hit = true;
            Cleanse cleanse = new Cleanse(1 , s);
            s.ApplyBuffStatusEffect(cleanse);
        }
        return hit;
    }
}
