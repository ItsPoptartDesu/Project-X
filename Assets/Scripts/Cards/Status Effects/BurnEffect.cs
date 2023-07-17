using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BurnEffect : StatusEffectHolder
{
    public BurnEffect(int _duration , Slime _affectedCharacter) : base(_duration , _affectedCharacter)
    {
        affectedCharacter = _affectedCharacter;
        myDebuffEffect = DeBuffStatusEffect.Burn;
    }

    public override void ApplyEffect()
    {
        UpdateEffect();
    }

    public override void RemoveEffect()
    {
    }

    public override void UpdateEffect()
    {
        affectedCharacter.TakeDamage(SlimeStats.BurnDamage);
        affectedCharacter.RefreshHealthBar();
        affectedCharacter.CheckDeath();
    }
}

