using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Play : FSM_State
{
    private bool toBattle = false;
    private bool toMainMenu = false;
    public FSM_Play()
    {
        stateID = StateID.Play;
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
        if (toBattle)
        {
            npc.PerformTransition(Transition.To_Battle);
        }
    }
    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Play DoBeforeEntering()");
        GameEntry.Instance.SetCurrentLevel(LevelTags.LEVEL_1);
        InGameUIController.OnClickGameToMainMenu += FSM_OnClickToMainMenu;
        SceneLoader.OnTransPlayToBattle += FSM_OnClickToBattle;
        LevelManager.Instance.OnPlayerEnterClean(
            ObjectManager.Instance.GetActivePlayerObject(),
            GameEntry.Instance.GetCurrentLevel());
        ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementRendererCamera();
        LevelManager.Instance.currentLevelBehaviour.PostLevelLoad();
    }
    public override void DoBeforeLeaving()
    {
        //ObjectManager.Instance.DeleteMarkedObjects();
        SceneLoader.OnTransPlayToBattle -= FSM_OnClickToBattle;
        Debug.Log("FSM_Play DoBeforeLeaving()");
        InGameUIController.OnClickGameToMainMenu -= FSM_OnClickToMainMenu;
        toMainMenu = false;
        toBattle = false;
        LevelManager.Instance.Load();
        ObjectManager.Instance.GetActivePlayer().DisablePlayerMovementRendererCamera();
        ObjectManager.Instance.GetActivePlayer().
            SetPreviousLevel(GameEntry.Instance.GetCurrentLevel());
    }
    private void FSM_OnClickToMainMenu()
    {
        toMainMenu = true;
    }
    private void FSM_OnClickToBattle()
    {
        toBattle = true;
    }
}