using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class BaseNPC : MonoBehaviour
{
    public BaseNPC() { }
    public BaseAI BattleBehaviour { get; set; }
    public abstract void OnEncounterStart(UI_ManaDisplay _manaDisplay);
    public abstract List<Slime> GetActiveTeam();
    protected SpriteRenderer[] myRenderers;
    protected Rigidbody2D myRigidbody;
    public void ToggleRenderers(bool _isOn)
    {
        if (myRenderers == null)
            myRenderers = GetComponents<SpriteRenderer>();

        foreach (var r in myRenderers)
            r.enabled = _isOn;
    }
}
