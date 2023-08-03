using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LevelBehavior : MonoBehaviour
{
    [Header("Base Class")]
    public LevelTags myLevelTag;
    //TODO:probably should do dictionary sorting or use some struct. but system.linq is nice 
    public List<SpawnPoints> spawnPoints = new List<SpawnPoints>();
    public UI_Base inGameUIController;
    public List<NPC_Trainer> npc_Trainers = new List<NPC_Trainer>();
    public Camera lvlCamera;
    protected Queue<CardDisplay> ActionQueue = new Queue<CardDisplay>();

    public virtual void PostLevelLoad()
    {

    }
    public List<CardDisplay> ShuffleDeck(List<CardDisplay> _toBeShuffled)
    {
        List<CardDisplay> shuffled = new List<CardDisplay>(_toBeShuffled);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            //int k = rnd.Next(i + 1);
            int k = Random.Range(0 , i + 1);
            CardDisplay value = shuffled[k];
            shuffled[k] = shuffled[i];
            shuffled[i] = value;
        }
        return shuffled;
    }
    public void SetPlayerToSpawnPoint(GameObject _player)
    {
        SpawnPoints sp = spawnPoints.First(x => x.GetObjectType() == ObjectType.Player);
        _player.transform.position = sp.position;
    }
    public List<SpawnPoints> GetSpawnPointsByType(ObjectType _type)
    {
        return spawnPoints.Where(x => x.GetObjectType() == _type).ToList();
    }
    public void MoveToRandomSpawnPoint(GameObject _player)
    {
        int Index = Random.Range(0, spawnPoints.Count);
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
    public void Load()
    {
        if (lvlCamera != null)
            lvlCamera.enabled = true;
    }
    public virtual void AddCardToActionQueue(CardDisplay _card) { Debug.LogError("Using Default AddCardToActionQueue"); }
    protected virtual void AddCardToDiscardPile(CardDisplay _card) { Debug.LogError("Using Default AddCardToDiscardPile"); }
    public void ToggleSettingsUI()
    {
        ObjectManager.Instance.GetActivePlayer().ToggleSettingsUI();
    }
}
