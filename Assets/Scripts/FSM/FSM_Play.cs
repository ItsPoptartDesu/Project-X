using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Play : FSM_State
{
    private bool toMainMenu = false;
    private bool toBattle = false;
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
        if (toBattle)
        {
            npc.PerformTransition(Transition.To_Battle);
        }
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Play DoBeforeEntering()");
        InGameUIController.OnClickGameToMainMenu += FSM_OnClickToMainMenu;
        GameEntry.PlayStateToBattleState += FSM_PlayToBattleTransition;

    }
    public override void DoBeforeLeaving()
    {
        ObjectManager.Instance.DeleteMarkedObjects();
        Debug.Log("FSM_Play DoBeforeLeaving()");
        InGameUIController.OnClickGameToMainMenu -= FSM_OnClickToMainMenu;
        GameEntry.PlayStateToBattleState -= FSM_PlayToBattleTransition;
        toMainMenu = false;
        toBattle = false;
    }
    private void FSM_OnClickToMainMenu()
    {
        toMainMenu = true;
    }
    private void FSM_PlayToBattleTransition()
    {
        toBattle = true;
    }
}
