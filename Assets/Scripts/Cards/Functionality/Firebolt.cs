using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : SlimeCard
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
        /// inc shields
        rawCardStats.GetHost().AdjustShields(rawCardStats.GetPower());
        //toggle status effect
        rawCardStats.GetHost().stats.SetStatus(StatusEffect.Thorn);
    }
}
