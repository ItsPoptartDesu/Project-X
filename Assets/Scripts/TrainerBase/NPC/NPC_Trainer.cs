using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPC_Trainer : MonoBehaviour
{
    public BattleBehaviour battleBehaviour;
    [SerializeField]
    [Range(2f, 10f)]
    private float LookDistance = 2;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField]
    private Transform rayCastPoint;
    [SerializeField]
    Vector2 LookDir = Vector2.left;
    [SerializeField]
    JSONTrainerInfo trainerInfo;
    bool hasBeenBattled = false;
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
        Debug.DrawRay(rayCastPoint.position, LookDir * LookDistance);
        var hit = Physics2D.Raycast(rayCastPoint.position, LookDir * LookDistance);
        if (hit.collider != null && !hasBeenBattled && !hasBeenBattled)
        {
            hasBeenBattled = true;
            Debug.Log($"Hit: {hit.transform.gameObject.name}");
            LevelManager.Instance.StartBattle(this, hit.transform.gameObject.GetComponent<PlayerController>());
        }
    }

    public void LoadTrainerData(JSONTrainerInfo _trainerData)
    {
        Debug.Log("LoadTrainerData");
        trainerInfo = _trainerData;

    }
    public void OnBattleStart(List<SpawnPoints> _spawnPoints, NPC_BattleSystem _system)
    {
        var ActiveList = trainerInfo.teamInfo.SavedSlime;
        foreach (var slime in ActiveList)
        {
            var NPC_Slime = ObjectManager.Instance.GenerateSlime(slime);
            Slime slimeComp = NPC_Slime.GetComponent<Slime>();
            int pos = (int)slimeComp.dna.TeamPos;
            slimeComp.AttachParent(_spawnPoints[pos].transform);
            slimeComp.transform.localScale = new Vector3(
                -ObjectManager.Instance.BattleScale,
                ObjectManager.Instance.BattleScale,
                ObjectManager.Instance.BattleScale);
            _system.CreateDecks(slimeComp, DECK_SLOTS.NPC);
        }
    }
}