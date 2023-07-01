using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESlimePart
{
    EARS,
    EYES,
    MOUTH,
    BACK,
    BODY,
    TAIL,
    FOREHEAD,
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
}
