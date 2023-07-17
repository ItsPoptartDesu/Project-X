using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Gun : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;

        int index = Random.Range(0, _activeTeam.Count);
        _activeTeam[index].ApplyDamage(rawCardStats);
        return true;
    }
}
