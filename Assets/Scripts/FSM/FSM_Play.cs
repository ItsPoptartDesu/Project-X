using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Play : FSM_State
{
    private bool toMainMenu = false;
    public FSM_Play()
    {
        stateID = StateID.Play;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
        if (toMainMenu)
        {
            npc.PerformTransition(Transition.To_Idle);
        }
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Play DoBeforeEntering()");
        InGameUIController.OnClickGameToMainMenu += FSM_OnClickToMainMenu;
    }
    public override void DoBeforeLeaving()
    {
        Debug.Log("FSM_Play DoBeforeLeaving()");
        InGameUIController.OnClickGameToMainMenu -= FSM_OnClickToMainMenu;
        toMainMenu = false;
    }
    private void FSM_OnClickToMainMenu()
    {
        toMainMenu = true;
    }
}
