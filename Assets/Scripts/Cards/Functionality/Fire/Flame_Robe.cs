using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_Robe : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;
        return true;
    }
}
