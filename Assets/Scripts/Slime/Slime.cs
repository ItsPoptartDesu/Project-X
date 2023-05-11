using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization;

[System.Serializable]
public enum BoardPos
{
    [EnumMember(Value = "Front 1")]
    F1 = 0,
    [EnumMember(Value = "Front 2")]
    F2,
    [EnumMember(Value = "Mid 1")]
    M1,
    [EnumMember(Value = "Mid 2")]
    M2,
    [EnumMember(Value = "Back 1")]
    B1,
    [EnumMember(Value = "Back 2")]
    B2,
    [EnumMember(Value = "NOT_SET")]
    NA,
}

public class Slime : MonoBehaviour
{
    public SlimeStats stats;
    private bool isDead = false;
    public bool IsDead() { return isDead; }

    [HideInInspector]
    public string secret;

    [SerializeField]
    private List<SlimePiece> _RenderParts = new List<SlimePiece>();

    private Dictionary<ESlimePart , SlimePiece> slimeParts = new Dictionary<ESlimePart , SlimePiece>();
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
        HealthBarRef.SetStats(stats.GetHealth() , stats.GetShield());
    }
    public int GetHealth()
    {
        return stats.GetHealth();
    }
    public int GetShields()
    {
        return stats.GetShield();
    }
    public void Init(JsonSlimeInfo _copy)
    {
        secret = _copy == null ? System.Guid.NewGuid().ToString() : _copy.secret;
        stats = new SlimeStats(_copy);
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
            p.SetHost(this);
            slimeParts.Add(p.GetESlimePart() , p);
        }
        System.Array values = System.Enum.GetValues(typeof(BoardPos));
        System.Random random = new System.Random();
        myBoardPos = _copy.TeamPos;
        isDead = false;
    }
    public void UpdateSlimePart(ESlimePart _piece , SO_SlimePart _part)
    {
        SlimePiece sp = slimeParts[_piece];
        sp.UpdateSlimePart(_part);
    }
    public List<SlimePiece> GetActiveParts()
    {
        return slimeParts.Values.ToList();
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
            statement += $"{p.GetSlimePartName()}, ";
        }
        statement += "attached to it";
        Debug.Log(statement);
    }
    public void ApplyDamage(SlimePiece _aggressor)
    {
        int damage = _aggressor.GetPower();

        //if i have the thorn effect toggled on
        if ((stats.GetStatus() & StatusEffect.Thorn) != 0)
        {
            _aggressor.GetHost().stats.TakeDamage((int)(damage * SlimeStats.ThornReturnPercentage));
            _aggressor.GetHost().RefreshHealthBar();
        }

        Vector2 hp = stats.TakeDamage(damage);
        if (hp.x <= 0)
            Die();
        HealthBarRef.SetHealth(hp);
    }
    public void AdjustShields(int _amount)
    {
        stats.AdjustShields(_amount);
        HealthBarRef.SetHealth(new Vector2(GetHealth() , _amount));
    }
    private void Die()
    {
        HealthBarRef.SetHealth(Vector2.zero);
        isDead = true;
        Debug.Log($"{stats.dna.SlimeName} has died");
    }
    public void RefreshHealthBar()
    {
        HealthBarRef.SetHealth(new Vector2(stats.GetHealth() , stats.GetShield()));
    }
}
