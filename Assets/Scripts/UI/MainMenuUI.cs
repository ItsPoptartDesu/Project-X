using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UI_Base
{
    public static MainMenuUI Instance { get; private set; }
    public UISlimeCollectionController teamSelectionManager;
    [SerializeField]
    private GameObject MenuIdle_UI;
    [SerializeField]
    private GameObject MenuCollection_UI;
    [SerializeField]
    private GameObject MenuSavedSlot_UI;
    [SerializeField]
    UISlimeCollectionController slimeCollectionController_UI;

    public Button PlayButton;
    public Button CollectionButton;

    public List<Slime> GetUISlimes()
    {
        return slimeCollectionController_UI.GetActiveTeam();
    }
    public void ShowSavedSlotUI()
    {
        MenuCollection_UI.SetActive(false);
        MenuIdle_UI.SetActive(false);
        MenuSavedSlot_UI.SetActive(true);
    }
    public void ResetUI()
    {
        MenuCollection_UI.SetActive(false);
        MenuSavedSlot_UI.SetActive(false);
        MenuIdle_UI.SetActive(true);
    }
    public void ShowCollectionUI()
    {
        MenuCollection_UI.SetActive(true);
        MenuSavedSlot_UI.SetActive(false);
        MenuIdle_UI.SetActive(false);
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        PlayButton.onClick.AddListener(GameEntry.Instance.Button_OnClickToPlay);
        CollectionButton.onClick.AddListener(GameEntry.Instance.Button_OnClickToCollectionClick);

    }

    public override void ToggleSelf()
    {
    }

    public override void DisableInGameUI()
    {
    }
}
