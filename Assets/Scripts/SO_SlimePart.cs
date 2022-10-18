using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Slime_Part
{
    EARS,
    EYES,
    MOUTH,
    BACK,
    BODY,
    TAIL,
    FOREHEAD,
}

public enum Slime_Type
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
    public Slime_Part SlimePart;
    public Slime_Type SlimeType;
    public string PartName;
    public Sprite ImgToDisplay;
    public float Power;
    public int Cost;
}
