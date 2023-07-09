using System.Collections;
using UnityEngine;
[System.Flags]
[System.Serializable]
public enum StatusEffect
{
    None = 0,
    Burn = 1,
    Poison = 2,
    Freeze = 4,
    Paralyze = 8,
    Thorn = 16,
}
/// <summary>
/// too be used for future stat saving and what not
/// slime kill count and other stuff to be added
/// will hold the dna of the slime and all its SO stats to be called on by the slime
/// </summary>
[System.Serializable]
public class SlimeStats
{
    public static float ThornReturnPercentage = 0.75f;
    public static int BurnDamage = 3;
    [HideInInspector]
    public JsonSlimeInfo dna;
    private int BaseHP = 100;
    private int RawHealth;
    private int HealthModifier = 0;
    private int BaseShield = 0;
    private int RawShield;
    private int ShieldModifier = 0;
    [SerializeField]
    private StatusEffect myStatus = StatusEffect.None;
    public StatusEffect GetStatus() { return myStatus; }
    public void SetStatus(StatusEffect status) { myStatus = status; }
    public int GetHealth() { return RawHealth; }

    public void TakeDamage(int _damage)
    {
        int diff = System.Math.Min(RawShield, _damage);
        RawShield -= diff;
        RawHealth -= _damage - diff;
    }
    public int GetShield() { return RawShield; }
    public void AdjustShields(int _shield)
    {
        RawShield += _shield;
    }
    public SlimeStats(JsonSlimeInfo _copy)
    {
        dna = _copy;
        RawHealth = BaseHP + HealthModifier;
        RawShield = BaseShield + ShieldModifier;
    }

}
