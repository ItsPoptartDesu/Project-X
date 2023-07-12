using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESlimePart
{
    HEAD,
    ARMS,
    LEGS,
    CHEST,
    HEART,
}

public enum ESlimeType
{
    BODY,
    Fire,
}

[CreateAssetMenu(fileName = "New Part", menuName = "Body Part")]
public class SO_SlimePart : ScriptableObject
{
    public ESlimePart SlimePart;
    public ESlimeType SlimeType;
    public CardComponentType CardComponentType;
    public string PartName;
    public Sprite CardArt;
    public Sprite SlimeArt;
    public int Power;
    public int Cost;
    [Range(0f , 1f)]
    public float StatusEffectProbability;
    [Range(0f,1f)]
    public float Accuracy;
}
