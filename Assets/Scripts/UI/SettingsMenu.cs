using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : UI_Base
{
    [SerializeField]
    private Canvas UIRoot;

    public override void DisableInGameUI()
    {
        UIRoot.enabled = false;
    }

    public override void ToggleSelf()
    {
        UIRoot.enabled = !UIRoot.enabled;
    }
}
