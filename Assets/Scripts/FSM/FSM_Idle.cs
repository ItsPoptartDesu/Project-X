using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Idle : FSM_State
{
    private bool toSettings = false;
    private bool toCollection = false;
    private bool toGame = false;
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
        if (toGame)
        {
            npc.PerformTransition(Transition.To_Play);
        }
    }
    public override void DoBeforeEntering()
    {
        GameEntry.Instance.SetCurrentLevel(LevelTags.MainMenu);
        GameEntry.OnClickCollectionMenu += FSM_OnCollectionClick;
        SceneLoader.OnClickToGame += FSM_OnClickToGame;
        Debug.Log("FSM_Idle DoBeforeEntering()");
    }
    private void FSM_OnClickToGame()
    {
        toGame = true;
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
        GameEntry.OnClickCollectionMenu -= FSM_OnCollectionClick;
        SceneLoader.OnClickToGame -= FSM_OnClickToGame;
        toSettings = false;
        toCollection = false;
        toGame = false;
        Debug.Log("FSM_Idle DoBeforeLeaving()");
        //ObjectManager.Instance.DeleteMarkedObjects();
        ObjectManager.Instance.GetActivePlayer().
            SetPreviousLevel(GameEntry.Instance.GetCurrentLevel());
    }
}
