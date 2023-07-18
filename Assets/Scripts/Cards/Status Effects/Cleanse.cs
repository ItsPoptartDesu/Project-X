public class Cleanse : StatusEffectHolder
{
    public Cleanse(int _turns , Slime affectedCharacter) : base(_turns , affectedCharacter)
    {
        myDebuffEffect = DeBuffStatusEffect.None;
        myBuffStatusEffects = BuffStatusEffects.Cleanse;
    }

    public override void ApplyEffect()
    {
        affectedCharacter.Cleanse();
    }

    public override void RemoveEffect()
    {
    }

    public override void UpdateEffect()
    {
    }
}
