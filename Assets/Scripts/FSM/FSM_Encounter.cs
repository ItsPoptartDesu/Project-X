using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Encounter : FSM_State
{
    private bool toPlay = false;
    public FSM_Encounter()
    {
        stateID = StateID.Encounter;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
        if(toPlay)
        {
            npc.PerformTransition(Transition.To_Play);
        }
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }
    public override void DoBeforeEntering()
    {
    }
    public override void DoBeforeLeaving()
    {
        toPlay = false;
    }
    private void FSM_OnTransitionToPlay()
    {
        toPlay = true;
    }
}
