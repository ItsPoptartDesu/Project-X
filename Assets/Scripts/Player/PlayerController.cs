using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TopDownCharacterController playerMovement;
    private SpriteRenderer[] myRenderers;
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
        if (myRenderers == null)
            myRenderers = GetComponents<SpriteRenderer>();
        foreach (var r in myRenderers)
        {
            r.enabled = !r.enabled;
        }
    }
    public void TogglePlayerMovement()
    {
        playerMovement.enabled = !playerMovement.enabled;
    }
    public void FirstLoad()
    {
        if (myRenderers == null)
            myRenderers = GetComponents<SpriteRenderer>();
        if (playerMovement == null)
            playerMovement = GetComponent<TopDownCharacterController>();
        //turn the player off
        ToggleRenderers(false);
        TogglePlayerMovement();//turn it off untill we load a level
        //load slimes
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
}
