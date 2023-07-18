using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sunny_Day : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        List<Slime> hurtTeam = GameEntry.Instance.GetActiveTeam()
            .Where(s => s.GetIsInjured()).ToList();

        if (hurtTeam.Count == 0)
            return true;

        int randomIndex = Random.Range(0 , hurtTeam.Count);
        List<int> hits = NPC_BattleSystem.GenerateNonRepeatingNumbers(0 ,
            hurtTeam.Count - 1 ,
            2);
        if (Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;
        for (int i = 0; i < hits.Count; i++)
            hurtTeam[i].Heal(rawCardStats.GetPower());

        return true;
    }
}
