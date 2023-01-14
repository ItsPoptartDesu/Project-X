using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spider_Web : SlimeCard
{
    public override void OnPlay(List<Slime> ActiveTeam)
    {
        base.OnPlay(ActiveTeam);
        Slime hit = ActiveTeam.OrderBy(x => x.dna.TeamPos).First();
        hit.ApplyDamage(rawCardStats.GetPower());
    }
}
