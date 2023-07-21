using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Encounter : FSM_State
{
    private bool toPlay = false;
    public FSM_Encounter()
    {
        stateID = StateID.Encounter;
    }
    public override void Act(Behaviour player, FSM_System npc)
    {
        if(toPlay)
        {
            npc.PerformTransition(Transition.To_Play);
        }
    }

    public override void Reason(Behaviour player, FSM_System npc)
    {
    }
    public override void DoBeforeEntering()
    {
        GameEntry.Instance.SetCurrentLevel(LevelManager.Instance.LoadingLevel);
        LevelManager.Instance.OnPlayerEnterClean(
                ObjectManager.Instance.GetActivePlayerObject() ,
                GameEntry.Instance.GetCurrentLevel());
        LevelManager.Instance.GetCurrentLevelBehavior().PostLevelLoad();
    }
    public override void DoBeforeLeaving()
    {
        toPlay = false;
        Debug.Log("FSM_Battle DoBeforeLeaving()");
        SceneLoader.OnTransBattleToPlay -= FSM_OnTransitionToPlay;
        LevelManager.Instance.Load();
        ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementRendererCamera();
        ObjectManager.Instance.GetActivePlayer().
            SetPreviousLevel(GameEntry.Instance.GetCurrentLevel());
    }
    private void FSM_OnTransitionToPlay()
    {
        toPlay = true;
    }
}
