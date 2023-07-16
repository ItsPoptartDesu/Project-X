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
    public override void OnPlay(List<Slime> _activeTeam)
    {
        if (Random.value > rawCardStats.GetAccuracy())
            return;
        _activeTeam[0].ApplyDamage(rawCardStats);
        //if i already have burn applyed to me dont re-add it
        if ((_activeTeam[0].GetStatusEffect() & rawCardStats.GetOnHitStatusEffect()) == StatusEffect.Burn)
            return;
        if (Random.value < rawCardStats.GetStatusEffectProbability())
        {
            BurnEffect burn = new BurnEffect(-1 , _activeTeam[0] , SlimeStats.BurnDamage);
            _activeTeam[0].ApplyStatusEffect(burn);
        }
    }
}
