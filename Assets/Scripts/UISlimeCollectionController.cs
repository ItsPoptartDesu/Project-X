using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlimeCollectionController : MonoBehaviour
{
    [SerializeField]
    private Transform ActiveTeamRoot;
    [SerializeField]
    private Transform RandomSpawnRoot;
    private GameObject SpawnedObject = null;

    private List<Slime> ActiveTeam_UI;
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
    private void Start()
    {
        ActiveTeam_UI = new List<Slime>();
    }
    public void GenerateRandomSlime()
    {
        if (SpawnedObject != null)
            Destroy(SpawnedObject);
        SpawnedObject = ObjectManager.Instance.GenerateRandomSlime();
        UIManager.Instance.teamSelectionManager.OnSpawnAttachment(SpawnedObject.transform);
    }
    public void OnClickAddToTeam()
    {
        if (SpawnedObject == null)
            return;
        ActiveTeam_UI.Add(SpawnedObject.GetComponent<Slime>());
        UIManager.Instance.teamSelectionManager.AttachNewMember(SpawnedObject.transform);
        SpawnedObject = null;
    }
    public void OnClickSaveTeam()
    {
        JsonSaveData jsd = GameEntry.Instance.GetSaveManager().GetSaveSlotOne();
    }
}
