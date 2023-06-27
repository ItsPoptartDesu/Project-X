using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : UI_Base
{
    public Button QuitButton;
    [SerializeField]
    Canvas myDisplay;

    public void Start()
    {
        QuitButton.onClick.AddListener(OnClick_QuitButton);
        DisableInGameUI();
    }
    public override void DisableInGameUI()
    {
        myDisplay.enabled = false;
    }

    public override void ToggleSelf()
    {
        myDisplay.enabled = !myDisplay.enabled;
    }

    private void OnClick_QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
