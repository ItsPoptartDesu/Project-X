using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Idle : FSM_State
{
    private bool toSettings = false;
    private bool toCollection = false;
    public FSM_Idle()
    {
        stateID = StateID.Idle;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {

    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
        if (toCollection)
        {
            npc.PerformTransition(Transition.To_Collection);
        }
    }

    public override void DoBeforeEntering()
    {
        GameEntry.OnCollectionMenuClick += FSM_OnCollectionClick;
        Debug.Log("FSM_Idle DoBeforeEntering()");
    }

    private void FSM_OnSettingsClick()
    {
        toSettings = true;
    }
    private void FSM_OnCollectionClick()
    {
        toCollection = true;
    }
    public override void DoBeforeLeaving()
    {
        GameEntry.OnCollectionMenuClick -= FSM_OnCollectionClick;
        toSettings = false;
        Debug.Log("FSM_Idle DoBeforeLeaving()");
    }
}
