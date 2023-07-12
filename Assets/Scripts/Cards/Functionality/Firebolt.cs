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
        _activeTeam[0].ApplyDamage(rawCardStats);
        if (Random.value < rawCardStats.GetStatusEffectProbability())
            _activeTeam[0].ApplyStatusEffect(StatusEffect.Burn);
    }
}
