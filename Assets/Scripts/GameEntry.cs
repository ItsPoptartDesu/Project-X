using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameEntry : MonoBehaviour
{
    public static GameEntry Instance { get; private set; }
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
    public bool isDEBUG = false;

    [SerializeField]
    private List<Sprite> FOREHEADSPRITES = new List<Sprite>();

    private Dictionary<string, Sprite> forehead_sprites = new Dictionary<string, Sprite>();
    private void LoadAssets()
    {
        foreach (var s in FOREHEADSPRITES)
        {
            if (forehead_sprites.ContainsKey(s.name))
            {
                Debug.LogWarning($"extra {s.name} added to FOREHEADSPRITES");
                continue;
            }
            if (isDEBUG)
                Debug.Log($"adding {s.name} to forehead sprites");
            forehead_sprites.Add(s.name, s);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadAssets();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject SlimePrefab;
    public void SpawnSlime()
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime toBeSpawned = slimePrefab.GetComponent<Slime>();
        Sprite ToBeRendered = forehead_sprites.Values.ElementAt(UnityEngine.Random.Range(0, forehead_sprites.Values.Count));
        //toBeSpawned.AddPart(forehead);
    }
}
