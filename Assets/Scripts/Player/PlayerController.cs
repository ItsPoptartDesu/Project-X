using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string UserName = "PeachRings";
    public string GetUsername() { return UserName; }
    public void SetUsername(string _name) { UserName = _name; }

    private PlayerMovement playerMovement;
    private SpriteRenderer[] myRenderers;
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private Camera myCamera;

    public void OnBattleStart(List<SpawnPoints> spawnPoints)
    {
        
    }
    public void DisablePlayerMovementAndRenderer()
    {
        TogglePlayerMovement(false);
        ToggleRenderers(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickOpenMenu();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            var at = GameEntry.Instance.GetActiveTeam();
            foreach (var slime in at)
            {
                slime.DebugStatement();
            }
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TogglePlayerMovement(true);
        }
    }
    public void AttachToSelf(Transform _toBeAttached)
    {
        _toBeAttached.parent = transform;
    }
    public void ToggleRenderers(bool _isOn)
    {
        foreach (var r in myRenderers)
            r.enabled = _isOn;
    }
    public void ToggleCamera(bool _isOn)
    {
        myCamera.enabled = _isOn;
    }
    public void TogglePlayerMovement(bool _isOn)
    {
        playerMovement.enabled = _isOn;
        myRigidbody.velocity = Vector3.zero;
    }
    public void FirstLoad()
    {
        myRenderers = GetComponents<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        myRigidbody = GetComponent<Rigidbody2D>();
        //turn it off until we load a level
        ToggleRenderers(false);
        TogglePlayerMovement(false);
        ToggleCamera(false);
        //load slimes
        var SaveSlot = GameEntry.Instance.GetSaveManager().GetSaveSlotOne();
        //on a clean load we wont have any team to load
        if (SaveSlot == null)
            return;
        List<JsonSlimeInfo> slimeJSON = GameEntry.Instance.GetSaveManager().GetSaveSlotOne().SavedSlime;
        Debug.Log($"TeamSize: {slimeJSON.Count}");
        foreach (JsonSlimeInfo s in slimeJSON)
        {
            GameObject mySlime = ObjectManager.Instance.GenerateSlime(s, true);
            AttachToSelf(mySlime.transform);
            Slime slimeComp = mySlime.GetComponent<Slime>();
            slimeComp.ToggleRenderers();
            GameEntry.Instance.GetSaveManager().AddSlimeToTeam(slimeComp);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void OnClickOpenMenu()
    {
        LevelManager.Instance.GetCurrentLevelBehavior().ToggleInGameUI();
    }
    public void EnablePlayerMovementRendererCamera()
    {
        TogglePlayerMovement(true);
        ToggleRenderers(true);
        ToggleCamera(true);
    }
    public void DisablePlayerMovementRendererCamera()
    {
        TogglePlayerMovement(false);
        ToggleRenderers(false);
        ToggleCamera(false);
    }
}
