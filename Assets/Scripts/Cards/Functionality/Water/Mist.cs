using System.Collections.Generic;
using UnityEngine;

public class Mist : CardDisplay
{
    public float AccDebuffRate = .2f;
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;
        foreach (Slime s in _activeTeam)
        {
            AccDebuff acc = new AccDebuff(2 , s, AccDebuffRate);
            s.ApplyDebuffStatusEffect(acc);
        }
        return true;
    }
}