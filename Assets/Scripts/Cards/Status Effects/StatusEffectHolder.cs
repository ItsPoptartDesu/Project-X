using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

[System.Flags]
[System.Serializable]
public enum DeBuffStatusEffect
{
    [EnumMember(Value = "None")]
    None = 0,
    [EnumMember(Value = "Burn")]
    Burn = 1,
    [EnumMember(Value = "AccDebuff")]
    AccDebuff = 2,
    [EnumMember(Value = "Freeze")]
    Freeze = 4,
    [EnumMember(Value = "Paralyze")]
    Paralyze = 8,
}
[System.Flags]
[System.Serializable]
public enum BuffStatusEffects
{
    [EnumMember(Value = "None")]
    None = 0,
    [EnumMember(Value = "Cleanse")]
    Cleanse = 1,
}

public abstract class StatusEffectHolder
{
    protected int turns;
    protected Slime affectedCharacter;
    protected DeBuffStatusEffect myDebuffEffect;
    protected BuffStatusEffects myBuffStatusEffects;
    public DeBuffStatusEffect GetStatusEffect() { return myDebuffEffect; }
    public BuffStatusEffects GetBuffEffect() { return myBuffStatusEffects; }

    public StatusEffectHolder(int _turns , Slime affectedCharacter)
    {
        this.turns = _turns;
        this.affectedCharacter = affectedCharacter;
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
    public abstract void UpdateEffect();
}

