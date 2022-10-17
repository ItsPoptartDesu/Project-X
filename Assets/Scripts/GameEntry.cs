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
    private List<SO_SlimePart> SO_ForeheadParts = new List<SO_SlimePart>();

    private Dictionary<string, SO_SlimePart> forehead_sprites = new Dictionary<string, SO_SlimePart>();
    private void LoadAssets()
    {
        foreach (var s in SO_ForeheadParts)
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
    public GameObject SpawnSlime()
    {
        GameObject slimePrefab = Instantiate(SlimePrefab);
        Slime slimeComp = slimePrefab.GetComponent<Slime>();
        slimeComp.Init();

        Sprite ToBeRendered = forehead_sprites.Values.ElementAt
            (UnityEngine.Random.Range(0, forehead_sprites.Values.Count)).ImgToDisplay;
        slimeComp.UpdateSlimePart(Slime_Part.FOREHEAD, ToBeRendered);
        //toBeSpawned.AddPart(forehead);
        return slimePrefab;
    }
}
