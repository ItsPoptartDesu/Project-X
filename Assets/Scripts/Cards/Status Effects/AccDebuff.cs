using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AccDebuff : StatusEffectHolder
{
    float previousAccModifier = 0;
    float AccDebuffRate = 0;
    public AccDebuff(int _turns , Slime affectedCharacter , float _accDebuffRate) : base(_turns , affectedCharacter)
    {
        myDebuffEffect = DeBuffStatusEffect.AccDebuff;
        AccDebuffRate = _accDebuffRate;
        previousAccModifier = affectedCharacter.GetAccuracyModifier();
    }

    public override void ApplyEffect()
    {
        affectedCharacter.SetAccuracyModifier(AccDebuffRate);
    }

    public override void RemoveEffect()
    {
        affectedCharacter.SetAccuracyModifier(previousAccModifier);
        affectedCharacter.RemoveStatusEffect(this);
    }

    public override void UpdateEffect()
    {
        turns -= 1;
        if (turns == 0)
            RemoveEffect();
    }
}