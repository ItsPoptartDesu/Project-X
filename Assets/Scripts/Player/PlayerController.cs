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
        if(Input.GetKeyDown(KeyCode.F1))
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
    public void ToggleRenderers()
    {
        foreach (var r in myRenderers)
        {
            r.enabled = !r.enabled;
        }
    }
    public void TogglePlayerMovement(bool _isOn)
    {
        playerMovement.enabled = _isOn;
        myRigidbody.velocity = Vector3.zero;
    }
    public void FirstLoad()
    {
        if (myRenderers == null)
            myRenderers = GetComponents<SpriteRenderer>();
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        if (myRigidbody == null)
            myRigidbody = GetComponent<Rigidbody2D>();
        //turn it off until we load a level
        ToggleRenderers(false);
        TogglePlayerMovement(false);
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
    }
    public void OnClickOpenMenu()
    {
        LevelManager.Instance.GetCurrentLevelBehavior().ToggleInGameUI();
    }
    public void EnablePlayerMovementAndRenderer()
    {
        TogglePlayerMovement(true);
        ToggleRenderers(true);
    }
    public void DisablePlayerMovementAndRenderer()
    {
        TogglePlayerMovement(false);
        ToggleRenderers(false);
    }
}
