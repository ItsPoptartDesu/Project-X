using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Collection : FSM_State
{
    private bool toMainMenu;
    public FSM_Collection()
    {
        stateID = StateID.Collection;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {

    }
    public override void Reason(Behaviour player, FSM_System npc)
    {
        if (toMainMenu)
        {
            npc.PerformTransition(Transition.To_Idle);
        }
    }
    private void FSM_OnClickBack()
    {
        toMainMenu = true;
    }
    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Collection DoBeforeEntering()");
        GameEntry.OnClickUIToMainMenu += FSM_OnClickBack;
    }
    public override void DoBeforeLeaving()
    {
        GameEntry.OnClickUIToMainMenu -= FSM_OnClickBack;
        toMainMenu = false;
        Debug.Log("FSM_Collection DoBeforeLeaving()");
    }
}
