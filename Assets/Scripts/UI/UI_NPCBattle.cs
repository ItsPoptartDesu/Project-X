using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_NPCBattle : UI_Base
{
    public GameObject CardPrefabRoot;
    public Canvas UIRoot;
    public GameObject EscapeMenuRoot;
    [Header("UI")]
    public TextMeshProUGUI[] DiscardText = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] DrawPileText = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] InkedPileText = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] NameTagText = new TextMeshProUGUI[2];
    [Space(2)]
    [Header("Game Timer")]
    public TextMeshProUGUI Text_TurnCounter;
    public TextMeshProUGUI Text_Timer;
    public TextMeshProUGUI Text_PlayerTurnIndicator;
    [Space(2f)]
    [Header("Player")]
    public List<Transform> Player_Team;
    public Transform Player_Hand_Root;


    [Space(2f)]
    [Header("Trainer")]
    public List<Transform> Trainer_Team;
    public Transform Trainer_Hand_Root;

    public static event System.Action OnClickLeaveBattle;
    public void AddCardToDiscardPile(SlimeCard _ToBeAdded,int _size)
    {
        _ToBeAdded.transform.SetParent(DiscardText[(int)_ToBeAdded.myOwner].transform);
        DiscardText[(int)_ToBeAdded.myOwner].text = _size.ToString();
    }
    public override void DisableInGameUI()
    {
        UIRoot.enabled = false;
    }

    public override void ToggleSelf()
    {
        UIRoot.enabled = !UIRoot.enabled;
        EscapeMenuRoot.SetActive(!EscapeMenuRoot.activeSelf);
    }
    public void UpdateUIObject(DECK_SLOTS _who, int _deckSize, string _name)
    {
        DiscardText[(int)_who].text = "0";
        InkedPileText[(int)_who].text = "0";
        NameTagText[(int)_who].text = _name;
        DrawPileText[(int)_who].text = _deckSize.ToString();
    }
    public void UpdateTurnDisplay(DECK_SLOTS _who)
    {
        Text_PlayerTurnIndicator.text = _who == DECK_SLOTS.PLAYER ? "Player Turn" : "NPC Turn";
    }
}
