using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlimeCollectionController : UI_Base
{
    [SerializeField]
    private Transform ActiveTeamRoot;
    [SerializeField]
    private Transform RandomSpawnRoot;
    private GameObject SpawnedObject = null;

    private List<Slime> ActiveTeam_UI;

    public Button RandomButton;
    public Button AddToTeamButton;
    public Button SaveTeamButton;
    public Button BackButton;
    public void Start()
    {
        RandomButton.onClick.AddListener(GenerateRandomSlime);
        AddToTeamButton.onClick.AddListener(OnClickAddToTeam);
        SaveTeamButton.onClick.AddListener(OnClickSaveTeam);
        BackButton.onClick.AddListener(GameEntry.Instance.Button_OnClickToMenuIdle);
        ActiveTeam_UI = new List<Slime>();
    }
    public List<Slime> GetActiveTeam()
    {
        return ActiveTeam_UI;
    }
    public void AttachNewMember(Transform _toBeAttached)
    {
        _toBeAttached.localScale = new Vector3(13f, 13f, 0);
        _toBeAttached.SetParent(ActiveTeamRoot);
        //_toBeAttached.localPosition = Vector3.zero;
    }
    public void OnSpawnAttachment(Transform _toBeAttached)
    {
        _toBeAttached.SetParent(RandomSpawnRoot);
    }
    public void GenerateRandomSlime()
    {
        if (SpawnedObject != null)
            Destroy(SpawnedObject);
        SpawnedObject = ObjectManager.Instance.GenerateRandomSlime();
        MainMenuUI.Instance.teamSelectionManager.OnSpawnAttachment(SpawnedObject.transform);
    }
    public void OnClickAddToTeam()
    {
        if (SpawnedObject == null)
            return;
        ActiveTeam_UI.Add(SpawnedObject.GetComponent<Slime>());
        MainMenuUI.Instance.teamSelectionManager.AttachNewMember(SpawnedObject.transform);
        SpawnedObject = null;
    }
    public void OnClickSaveTeam()
    {
        
    }

    public override void ToggleSelf()
    {
    }

    public override void DisableInGameUI()
    {
    }
}
