using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Collection : FSM_State
{
    public FSM_Collection()
    {
        stateID = StateID.Collection;
    }

    public override void Act(Behaviour player, FSM_System npc)
    {
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }
    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Collection DoBeforeEntering()");
    }
    public override void DoBeforeLeaving()
    {
        Debug.Log("FSM_Collection DoBeforeLeaving()");
    }
}
