using System.Collections;
using UnityEngine;


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
    private float AccuracyModifier = 1f;
    [SerializeField]
    private DeBuffStatusEffect myDebuffStatus = DeBuffStatusEffect.None;
    public DeBuffStatusEffect GetDebuffStatus() { return myDebuffStatus; }
    public void SetDebuffStatus(DeBuffStatusEffect status) { myDebuffStatus = status; }
    public int GetHealth() { return RawHealth; }
    public float GetAccuracyModifier() {  return AccuracyModifier; }
    public float GetAccuracy(float _cardAcc) { return AccuracyModifier * _cardAcc; }
    public void SetAccuracyModifier(float _rate)
    {
        if(_rate < 0)
        {
            AccuracyModifier = 0;
            return;
        }
        if(_rate > 1)
        {
            AccuracyModifier = 1;
            return;
        }
        AccuracyModifier *= _rate;
    }
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
