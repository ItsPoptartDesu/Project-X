using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Gun : CardDisplay
{
    public override void OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetAccuracy())
            return;

        int index = Random.Range(0, _activeTeam.Count);
        _activeTeam[index].ApplyDamage(rawCardStats);
    }
}
