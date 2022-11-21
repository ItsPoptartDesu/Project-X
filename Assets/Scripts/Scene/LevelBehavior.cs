using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelBehavior : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public InGameUIController inGameUIController;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToFirstSpawnPoint(GameObject _player)
    {
        _player.transform.position = spawnPoints[0].transform.position;
    }

    public void MoveToRandomSpawnPoint(GameObject _player)
    {
        System.Random r = new System.Random();
        int Index = r.Next(0, spawnPoints.Count);
        _player.transform.position = spawnPoints[Index].position;
    }

    public void ToggleInGameUI()
    {
        inGameUIController.ToggleSelf();
    }
    public void DisableInGameIU()
    {
        inGameUIController.DisableInGameUI();
    }
}
