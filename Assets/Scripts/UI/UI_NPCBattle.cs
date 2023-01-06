using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_NPCBattle : UI_Base
{
    public GameObject CardPrefabRoot;
    public Canvas UIRoot;
    public GameObject EscapeMenuRoot;
    [Header("Game Timer")]
    public TextMeshProUGUI Text_TurnCounter;
    public TextMeshProUGUI Text_Timer;
    public TextMeshProUGUI Text_PlayerTurnIndicator;
    [Space(2f)]
    [Header("Player")]
    public TextMeshProUGUI Player_text_discardPile;
    public TextMeshProUGUI Player_text_drawPile;
    public TextMeshProUGUI Player_text_inkedPile;
    public List<Transform> Player_Team;
    public Transform Player_Hand_Root;
    [Space(2f)]
    [Header("Trainer")]
    public TextMeshProUGUI Trainer_text_discardPile;
    public TextMeshProUGUI Trainer_text_drawPile;
    public TextMeshProUGUI Trainer_text_inkedPile;
    public List<Transform> Trainer_Team;
    public Transform Trainer_Hand_Root;

    public static event System.Action OnClickLeaveBattle;

    public override void DisableInGameUI()
    {
        UIRoot.enabled = false;
    }

    public override void ToggleSelf()
    {
        UIRoot.enabled = !UIRoot.enabled;
        EscapeMenuRoot.SetActive(!EscapeMenuRoot.activeSelf);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
