using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public struct SceneLoaderInfo
{
    public string LevelName;
    public LevelTags LevelTag;
}

public class SceneLoader : MonoBehaviour
{
    public static event System.Action OnClickToGame;
    public static event System.Action OnTransPlayToBattle;
    public static event System.Action OnTransBattleToPlay;
    public static event System.Action OnTransToEncounter;
    //public static event System.Action OnTransToEncounter;
    public Dictionary<LevelTags, SceneLoaderInfo> SceneHash = new Dictionary<LevelTags, SceneLoaderInfo>();
    public List<SceneLoaderInfo> SceneName = new List<SceneLoaderInfo>();
    public void Start()
    {
        foreach (var n in SceneName)
        {
            if (SceneHash.ContainsKey(n.LevelTag))
            {
                Debug.LogError($"SceneHas already has {n.LevelTag} & {n.LevelName}");
                return;
            }
            SceneHash.Add(n.LevelTag, n);
        }
    }
    public void StartAsyncLoad(LevelTags _level)
    {
        if (GameEntry.Instance.LoadingPause)
            return;
        GameEntry.Instance.LoadingPause = true;
        if (GameEntry.Instance.isDEBUG)
            Debug.Log($"{SceneHash[_level].LevelName} & {_level}");
        StartCoroutine(LoadSceneAsync(SceneHash[_level].LevelName, _level));
        switch (_level)
        {
            case LevelTags.MainMenu:
                break;
            case LevelTags.LEVEL_1:
                OnClickToGame?.Invoke();
                OnTransBattleToPlay?.Invoke();
                break;
            case LevelTags.NPC_Battle:
                OnTransPlayToBattle?.Invoke();
                break;
            case LevelTags.ENCOUNTER:
                OnTransToEncounter?.Invoke();
                break;
            default:
                break;
        }
    }
    IEnumerator LoadSceneAsync(string _lvlName, LevelTags _level)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_lvlName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);//todo dont call new each time save a ref
        GameEntry.Instance.LoadingPause = false;
    }
}
