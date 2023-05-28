using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IGController : UI_Base
{
    public static event System.Action OnClickGameToMainMenu;

    [SerializeField]
    SettingsMenu settingsMenu;
    public SettingsMenu GetSettingsMenu() { return settingsMenu; }

    public void OnClick_GameToMainMenu()
    {
        OnClickGameToMainMenu?.Invoke();
    }
}
