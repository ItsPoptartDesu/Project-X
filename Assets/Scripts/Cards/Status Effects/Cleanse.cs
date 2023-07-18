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
        affectedCharacter.RemoveBuffStatusEffect(this);
        UnityEngine.Debug.Log("REMOVEING CLEANSE");
    }

    public override void UpdateEffect()
    {
        RemoveEffect();
    }
}
