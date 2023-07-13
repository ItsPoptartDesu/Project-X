using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_Robe : CardDisplay
{
    public override void OnPlay(List<Slime> _activeTeam)
    {
        rawCardStats.GetHost().ApplyStatusEffect(StatusEffect.Cleanse);
    }
}
