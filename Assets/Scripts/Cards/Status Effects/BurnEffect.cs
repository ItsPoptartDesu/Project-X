using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BurnEffect : StatusEffectHolder
{
    private int damagePerHit;
    public BurnEffect(int _duration , Slime _affectedCharacter, int _damagePerHit) : base(_duration , _affectedCharacter)
    {
        damagePerHit = _damagePerHit;
        affectedCharacter = _affectedCharacter;
        myEffect = StatusEffect.Burn;
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
        affectedCharacter.TakeDamage(damagePerHit);
        affectedCharacter.RefreshHealthBar();
        affectedCharacter.CheckDeath();
    }
}

