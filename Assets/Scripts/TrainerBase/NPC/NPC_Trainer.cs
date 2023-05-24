using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPC_Trainer : MonoBehaviour
{
    public BattleBehaviour battleBehaviour;
    public TrainerStatus trainerInfo;
    [SerializeField]
    [Range(2f , 10f)]
    private float LookDistance = 2;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField]
    private Transform rayCastPoint;
    [SerializeField]
    Vector2 LookDir = Vector2.left;
    public string GetTrainerName() { return trainerInfo.TrainerName; }
    public List<Slime> ActiveTeam = new List<Slime>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        LookDistance = 2;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        string trainerName = trainerInfo.TrainerName;
        if (trainerName == null || trainerName == string.Empty)
            return;
        if (trainerInfo.HasBeenBattled && !GameEntry.Instance.isDEBUG)
            return;
        Debug.DrawRay(rayCastPoint.position , LookDir * LookDistance);
        var hit = Physics2D.Raycast(rayCastPoint.position , LookDir * LookDistance);
        if (hit.collider != null)
        {
            Debug.Log($"Hit: {hit.transform.gameObject.name}");
            LevelManager.Instance.StartBattle(this , hit.transform.gameObject.GetComponent<PlayerController>());
        }
    }
    public void OnBattleStart(NPC_BattleSystem _system)
    {
        var ActiveList = trainerInfo.ActiveTeam.SavedSlime;
        Debug.Log($"{ActiveList.Count}");
        foreach (var slime in ActiveList)
        {
            var NPC_Slime = ObjectManager.Instance.GenerateSlime(slime);
            Slime slimeComp = NPC_Slime.GetComponent<Slime>();

            BoardPos pos = slimeComp.stats.dna.TeamPos;
            SpawnPoints sp = _system.GetSpawnPoint(DECK_SLOTS.NPC , pos);
            slimeComp.AttachParent(sp.transform);
            slimeComp.transform.localScale = new Vector3(
                -ObjectManager.Instance.BattleScale ,
                ObjectManager.Instance.BattleScale ,
                ObjectManager.Instance.BattleScale);
            _system.CreateDecks(slimeComp , DECK_SLOTS.NPC);
            HealthBar hb = _system.InitHealhBar(
                DECK_SLOTS.NPC ,
                pos ,
                new Vector2(slimeComp.GetHealth() , slimeComp.GetShields()));
            slimeComp.InitHealthBar(hb);
            ActiveTeam.Add(slimeComp);
        }
    }
    public void OnBattleEnd(WIN_STATE _state)
    {
        Debug.Log($"Battle Over State{_state}");
        if (_state == WIN_STATE.PLAYER_WIN)
            trainerInfo.HasBeenBattled = true;
        //GameEntry.Instance.GetSaveManager().UpdateTrainerStatus(trainerInfo.TrainerName);
    }
}