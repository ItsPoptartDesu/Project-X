using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        Slime closest = _activeTeam.OrderBy(x => x.myBoardPos).FirstOrDefault();
        closest.ApplyDamage(rawCardStats);
        closest.ApplyStatusEffect(StatusEffect.Burn);
        rawCardStats.GetHost().AdjustShields(rawCardStats.GetPower());
    }
}
