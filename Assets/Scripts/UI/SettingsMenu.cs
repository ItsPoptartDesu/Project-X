using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : UI_Base
{
    [SerializeField]
    private Canvas UIRoot;

    public Button OnClick;

    public override void DisableInGameUI()
    {
        UIRoot.enabled = false;
    }

    public override void ToggleSelf()
    {
        UIRoot.enabled = !UIRoot.enabled;
    }
}
