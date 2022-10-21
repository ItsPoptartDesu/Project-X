using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Play : FSM_State
{
    public FSM_Play()
    {
        stateID = StateID.Play;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }
}
