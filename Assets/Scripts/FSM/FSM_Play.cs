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

    }
    public override void Reason(Behaviour player, FSM_System npc)
    {
        if (toMainMenu)
        {
            npc.PerformTransition(Transition.To_Idle);
        }
    }
    public override void DoBeforeEntering()
    {
        Debug.Log("FSM_Play DoBeforeEntering()");
        GameEntry.Instance.SetCurrentLevel(LevelTags.LEVEL_1);
        InGameUIController.OnClickGameToMainMenu += FSM_OnClickToMainMenu;
        LevelManager.Instance.OnPlayerEnterClean(
            ObjectManager.Instance.GetActivePlayerObject(),
            GameEntry.Instance.GetCurrentLevel());
        ObjectManager.Instance.GetActivePlayer().EnablePlayerMovementRendererCamera();
        LevelManager.Instance.currentLevelBehaviour.PostLevelLoad();
    }
    public override void DoBeforeLeaving()
    {
        ObjectManager.Instance.DeleteMarkedObjects();
        Debug.Log("FSM_Play DoBeforeLeaving()");
        InGameUIController.OnClickGameToMainMenu -= FSM_OnClickToMainMenu;
        toMainMenu = false;
        toBattle = false;
        LevelManager.Instance.Load();
        ObjectManager.Instance.GetActivePlayer().DisablePlayerMovementRendererCamera();
        GameEntry.Instance.StartLoadLevel();
    }
    private void FSM_OnClickToMainMenu()
    {
        toMainMenu = true;
    }
}