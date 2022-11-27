using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Battle : FSM_State
{
    public FSM_Battle()
    {
        stateID = StateID.Battle;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }
    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Battle DoBeforeEntering()");
    }
    public override void DoBeforeLeaving()
    {
        Debug.Log("FSM_Battle DoBeforeLeaving()");
    }
}
