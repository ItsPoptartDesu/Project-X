using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPC_Trainer : BaseNPC
{
    public NPC_Trainer() : base()
    {
        Init();
    }
    public AI_LEVEL myAILevel;
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
    public override List<Slime> GetActiveTeam() { return ActiveTeam; }
    private List<Slime> ActiveTeam = new List<Slime>();
    public bool isDummy = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Init();
    }
    private void Init()
    {
        LookDistance = 2;
        UpdateBattleBehaviour();
    }
    public void UpdateBattleBehaviour()
    {
        switch (myAILevel)
        {
            case AI_LEVEL.EASY:
                BattleBehaviour = new EasyAI();
                break;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDummy)
            return;
        if (trainerInfo.HasBeenBattled)
            return;
        string trainerName = trainerInfo.TrainerName;
        if (trainerName == null || trainerName == string.Empty)
            return;
        Debug.DrawRay(rayCastPoint.position , LookDir * LookDistance);
        var hit = Physics2D.Raycast(rayCastPoint.position , LookDir * LookDistance);
        PlayerController pc = hit.transform.gameObject.GetComponent<PlayerController>();
        if (hit.collider != null && pc != null)
        {
            Debug.Log($"Hit: {hit.transform.gameObject.name}");
            trainerInfo.HasBeenBattled = true;
            LevelManager.Instance.StartEncounter(this , pc);
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

            BoardPos pos = slimeComp.GetDNA().TeamPos;
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
    public override void OnEncounterStart(UI_ManaDisplay _mana)
    {
        List<JsonSlimeInfo> team = trainerInfo.ActiveTeam.SavedSlime;
        BattleBehaviour.ManaDisplayReference = _mana;
        foreach (JsonSlimeInfo slime in team)
        {
            var NPC_Slime = ObjectManager.Instance.GenerateSlime(slime);
            Slime slimeComp = NPC_Slime.GetComponent<Slime>();
            ActiveTeam.Add(slimeComp);
        }
    }
}