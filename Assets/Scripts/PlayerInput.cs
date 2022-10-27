using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameObject SpawnedObject = null;
    private List<Slime> ActiveTeam;
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
        SpawnedObject = GameEntry.Instance.GenerateRandomSlime();
    }

    public List<Slime> GetActiveTeam()
    {
        ActiveTeam.Clear();
        ActiveTeam.Add(SpawnedObject.GetComponent<Slime>());
        return ActiveTeam;
    }
}
