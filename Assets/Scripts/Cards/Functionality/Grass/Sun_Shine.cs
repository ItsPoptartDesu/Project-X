using System.Collections.Generic;
using System.Linq;

public class Sun_Shine : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        List<Slime> hurtTeam = GameEntry.Instance.GetActiveTeam()
            .Where(s => s.GetIsInjured()).ToList();

        if (hurtTeam.Count == 0)
            return true;

        int randomIndex = UnityEngine.Random.Range(0 , hurtTeam.Count);

        if (UnityEngine.Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;

        hurtTeam[randomIndex].Heal(rawCardStats.GetPower());

        return true;
    }

}
