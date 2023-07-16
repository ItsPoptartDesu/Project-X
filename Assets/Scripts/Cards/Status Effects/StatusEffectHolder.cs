using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

[System.Flags]
[System.Serializable]
public enum StatusEffect
{
    [EnumMember(Value = "None")]
    None = 0,
    [EnumMember(Value = "Burn")]
    Burn = 1,
    [EnumMember(Value = "Poison")]
    Poison = 2,
    [EnumMember(Value = "Freeze")]
    Freeze = 4,
    [EnumMember(Value = "Paralyze")]
    Paralyze = 8,
    [EnumMember(Value = "Thorn")]
    Thorn = 16,
    [EnumMember(Value = "Cleanse")]
    Cleanse = 32,
}
public abstract class StatusEffectHolder
{
    protected int turns;
    protected Slime affectedCharacter;
    protected StatusEffect myEffect;
    public StatusEffect GetStatusEffect() { return myEffect; }

    public StatusEffectHolder(int _turns , Slime affectedCharacter)
    {
        this.turns = _turns;
        this.affectedCharacter = affectedCharacter;
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
    public abstract void UpdateEffect();
}

