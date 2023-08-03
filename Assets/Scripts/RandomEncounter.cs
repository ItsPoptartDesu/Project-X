using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    public List<JsonSlimeInfo> toBeSpawned;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (Random.Range(0 , 100) < 99)
            {
                NPC_Trainer t = ObjectManager.Instance.GetDummyTrainer();
                t.trainerInfo.ActiveTeam.SavedSlime.Clear();
                foreach (var s in toBeSpawned)
                    t.trainerInfo.ActiveTeam.SavedSlime.Add(s);
                LevelManager.Instance.StartEncounter(t , playerController);
                //playerController.FSM_Encounter(toBeSpawned);
            }
        }
    }
}
