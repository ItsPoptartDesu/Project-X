using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Settings : FSM_State
{
    public FSM_Settings()
    {
        stateID = StateID.Settings;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }
}
