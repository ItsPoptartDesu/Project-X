using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    private List<SlimePiece> _RenderParts = new List<SlimePiece>();

    private Dictionary<Slime_Part, SlimePiece> slimeParts = new Dictionary<Slime_Part, SlimePiece>();



    // Start is called before the first frame update
    void Start()
    {
        foreach (var p in _RenderParts)
        {
            if (slimeParts.ContainsKey(p.whichPart))
            {
                if (GameEntry.Instance.isDEBUG)
                {
                    Debug.LogWarning($"duplicate part added {p.whichPart + " || " + p.SlimePartName }");
                }
                continue;
            }
            slimeParts.Add(p.whichPart, p);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
