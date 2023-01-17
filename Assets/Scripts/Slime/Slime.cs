using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BoardPos
{
    F1 = 0, F2,
    M1, M2,
    B1, B2,
    NA,
}

public class Slime : MonoBehaviour
{
    private int BaseHP = 100;
    private int RawHealth;
    private int HealthModifier = 0;
    public int GetHealth() { return HealthModifier + RawHealth; }
    private bool isDead = false;
    public bool IsDead() { return isDead; }
    [HideInInspector]
    public JsonSlimeInfo dna;
    [HideInInspector]
    public string secret;

    [SerializeField]
    private List<SlimePiece> _RenderParts = new List<SlimePiece>();

    private Dictionary<ESlimePart, SlimePiece> slimeParts = new Dictionary<ESlimePart, SlimePiece>();
    private HealthBar HealthBarRef;
    public string SlimeName { get; private set; }
    public BoardPos myBoardPos = BoardPos.NA;
    public void AttachParent(Transform _parent)
    {
        transform.SetParent(_parent);
        GetComponent<RectTransform>().position = _parent.position;
    }
    public void SetPosition(Vector2 _pos)
    {
        GetComponent<RectTransform>().position = _pos;
    }
    public void InitHealthBar(HealthBar _bar)
    {
        HealthBarRef = _bar;
        HealthBarRef.SetMaxHealth(GetHealth());
    }
    public void Init(JsonSlimeInfo _copy)
    {
        dna = _copy;
        secret = _copy == null ? System.Guid.NewGuid().ToString() : _copy.secret;
        RawHealth = BaseHP;
        foreach (var p in _RenderParts)
        {
            if (slimeParts.ContainsKey(p.GetESlimePart()))
            {
                if (GameEntry.Instance.isDEBUG)
                {
                    Debug.LogWarning($"duplicate part added {p.GetESlimePart() + " || " + p.GetSlimePartName() }");
                }
                continue;
            }
            if (GameEntry.Instance.isDEBUG)
                Debug.Log($"adding part {p.GetESlimePart()} to slime {SlimeName}");
            slimeParts.Add(p.GetESlimePart(), p);
        }
        System.Array values = System.Enum.GetValues(typeof(BoardPos));
        System.Random random = new System.Random();
        myBoardPos = (BoardPos)values.GetValue(random.Next(values.Length));
        isDead = false;
    }
    public void UpdateSlimePart(ESlimePart _piece, SO_SlimePart _part)
    {
        SlimePiece sp = slimeParts[_piece];
        sp.UpdateSlimePart(_part);
    }
    public List<SlimePiece> GetActiveParts()
    {
        return slimeParts.Values.ToList();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleRenderers()
    {
        foreach (var piece in _RenderParts)
            piece.ToggleRenderer();
    }
    public void DebugStatement()
    {
        string statement = $"{SlimeName} has ";
        foreach (var p in slimeParts.Values)
        {
            statement += $"{p.GetSlimePartName()} ";
        }
        statement += "attached to it";
        Debug.Log(statement);
    }
    public void ApplyDamage(int _damage)
    {
        int hp = GetHealth();
        hp -= _damage;
        if (hp < 0)
            Die();
        else
            HealthBarRef.SetHealth(hp);
    }
    private void Die()
    {
        HealthBarRef.SetHealth(0);
        isDead = true;
        Debug.Log($"{dna.SlimeName} has died");
    }
}
