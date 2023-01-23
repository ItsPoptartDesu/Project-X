using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Battle : FSM_State
{
    private bool LeaveBattle = false;
    public FSM_Battle()
    {
        stateID = StateID.Battle;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
        if (LeaveBattle)
        {
            npc.PerformTransition(Transition.To_Play);
        }

    }
    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Battle DoBeforeEntering()");
        UI_NPCBattle.OnClickLeaveBattle += FSM_LeaveBattle;
        GameEntry.Instance.SetCurrentLevel(LevelTags.NPC_Battle);
    }
    public override void DoBeforeLeaving()
    {
        Debug.Log("FSM_Battle DoBeforeLeaving()");
        UI_NPCBattle.OnClickLeaveBattle -= FSM_LeaveBattle;
        LeaveBattle = false;
    }
    private void FSM_LeaveBattle()
    {
        LeaveBattle = true;
    }
}
