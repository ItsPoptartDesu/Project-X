using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BoardPos
{
    F1, F2,
    M1, M2,
    B1, B2,
    NA,
}

public class Slime : MonoBehaviour
{
    [SerializeField]
    private List<SlimePiece> _RenderParts = new List<SlimePiece>();

    private Dictionary<Slime_Part, SlimePiece> slimeParts = new Dictionary<Slime_Part, SlimePiece>();
    public string SlimeName { get; private set; }
    public BoardPos myBoardPos = BoardPos.NA;
    // Start is called before the first frame update
    public void Init()
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
            Debug.Log($"adding part {p.whichPart} to slime {SlimeName}");
            slimeParts.Add(p.whichPart, p);
        }
        System.Array values = System.Enum.GetValues(typeof(BoardPos));
        System.Random random = new System.Random();
        myBoardPos = (BoardPos)values.GetValue(random.Next(values.Length));
    }

    public void UpdateSlimePart(Slime_Part _piece, Sprite _toBeRendered)
    {
        SlimePiece sp = slimeParts[_piece];
        sp.ToBeRendered = _toBeRendered;
    }
    public List<SlimePiece> GetActiveParts()
    {
        return slimeParts.Values.ToList();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
