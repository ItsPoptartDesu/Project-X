using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
}
public class NPC_BattleSystem : LevelBehavior
{
    private PlayerController user;
    private NPC_Trainer npc;
    [SerializeField]
    private List<SpawnPoints> NPC_SpawnPoints;
    [SerializeField]
    private List<SpawnPoints> Player_SpawnPoints;
    public Canvas mainCanvas;
    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void PreLoadForBattle(PlayerController _player, NPC_Trainer _npc)
    {
        user = _player;
        npc = _npc;
        Debug.Log("PreLoadForBattle");
        _player.OnBattleStart(Player_SpawnPoints);
        state = BattleState.START;
    }
    public override void PostLevelLoad()
    {
        user.DisablePlayerMovementAndRenderer();
        StartCoroutine(SetupBattle());
        mainCanvas.worldCamera = user.GetCamera();
    }
    IEnumerator SetupBattle()
    {
        // get player obj
        // get trainer obj
        //update hud
        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        Debug.Log("PlayerTurn");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
