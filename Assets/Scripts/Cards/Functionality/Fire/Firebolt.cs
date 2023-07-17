using System.Collections.Generic;
using UnityEngine;

public class Firebolt : CardDisplay
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        SlimePiece cardStats = rawCardStats;
        Slime activeSlime = _activeTeam[0];

        if (Random.value > cardStats.GetHost().GetAccuracy(cardStats.GetAccuracy()))
            return false;

        activeSlime.ApplyDamage(cardStats);

        if ((activeSlime.GetDebuffStatus() & cardStats.GetOnHitStatusEffect()) == DeBuffStatusEffect.Burn)
            return true;

        if (Random.value < cardStats.GetStatusEffectProbability())
        {
            BurnEffect burn = new BurnEffect(-1 , activeSlime);
            activeSlime.ApplyStatusEffect(burn);
        }

        return true;
    }

}
