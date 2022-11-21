using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public static event System.Action OnClickGameToMainMenu;
    public Canvas UIRoot;
    public void OnClick_GameToMainMenu()
    {
        OnClickGameToMainMenu?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        UIRoot.enabled = false;
    }
    public void DisableInGameUI()
    {
        UIRoot.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleSelf()
    {
        UIRoot.enabled = !UIRoot.enabled;
    }
}
