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
}

public enum ESlimeType
{
    PSYCHIC, STEEL, ICE,
    WATER, FAIRY, BUG,
    GHOST, ROCK, GRASS,
    FIRE, DRAGON, POISON,
    DARK, GROUND, AIR,
    ELECTRIC, FLYING, FIGHTING,
    NORMAL,
}

[CreateAssetMenu(fileName = "New Part", menuName = "Body Part")]
public class SO_SlimePart : ScriptableObject
{
    public ESlimePart SlimePart;
    public ESlimeType SlimeType;
    public CardComponentType CardComponentType;
    public string PartName;
    public Sprite ImgToDisplay;
    public float Power;
    public int Cost;
}
