using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer[] myRenderers;
    public void ToggleRenderers(bool _isOn)
    {
        if (myRenderers == null)
            myRenderers = GetComponents<SpriteRenderer>();
        foreach (var r in myRenderers)
        {
            r.enabled = _isOn;
        }
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
    public void FirstLoad()
    {
        //turn the player off
        ToggleRenderers(false);
        //load slimes
    }
}
