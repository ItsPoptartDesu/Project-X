using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameObject SpawnedObject = null;
    private List<Slime> ActiveTeam;

    [SerializeField]
    private TeamSelectionManager teamSelectionManager;
    // Start is called before the first frame update
    void Start()
    {
        ActiveTeam = new List<Slime>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            GenerateRandomSlime();
        }
    }

    public void GenerateRandomSlime()
    {
        if (SpawnedObject != null)
            Destroy(SpawnedObject);
        SpawnedObject = ObjectManager.Instance.GenerateRandomSlime();
        SpawnedObject.transform.localScale = new Vector3(13f, 13f, 0);
        teamSelectionManager.OnSpawnAttachment(SpawnedObject.transform);
    }

    public void OnClickAddToTeam()
    {
        ActiveTeam.Add(SpawnedObject.GetComponent<Slime>());
        teamSelectionManager.AttachNewMember(SpawnedObject.transform);
        SpawnedObject = null;
    }

    public List<Slime> GetActiveTeam()
    {
        return ActiveTeam;
    }
}
