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
        LeaveBattle = false;
        Debug.Log("FSM_Battle DoBeforeEntering()");
        SceneLoader.OnTransBattleToPlay += FSM_LeaveBattle;
        GameEntry.Instance.SetCurrentLevel(LevelManager.Instance.LoadingLevel);
        LevelManager.Instance.OnPlayerEnterClean(
            ObjectManager.Instance.GetActivePlayerObject(),
            GameEntry.Instance.GetCurrentLevel());
        LevelManager.Instance.GetCurrentLevelBehavior().PostLevelLoad();
    }
    public override void DoBeforeLeaving()
    {
        LeaveBattle = false;
        Debug.Log("FSM_Battle DoBeforeLeaving()");
        SceneLoader.OnTransBattleToPlay -= FSM_LeaveBattle;
        LevelManager.Instance.Load();
        ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementRendererCamera();
        ObjectManager.Instance.GetActivePlayer().
            SetPreviousLevel(GameEntry.Instance.GetCurrentLevel());
    }
    private void FSM_LeaveBattle()
    {
        LeaveBattle = true;
    }
}
