using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

[System.Serializable]
public enum CardComponentType
{
    [EnumMember(Value = "BODY")]
    BODY,
    [EnumMember(Value = "Flame_Robe")]
    Flame_Robe,
    [EnumMember(Value = "Embers")]
    Embers,
    [EnumMember(Value = "Fire_Snake")]
    Fire_Snake,
    [EnumMember(Value = "Firebolt")]
    Firebolt,
    [EnumMember(Value = "Firewall")]
    Firewall, 
    [EnumMember(Value = "Water_Gun")]
    Water_Gun,
    [EnumMember(Value = "Mist")]
    Mist,
    [EnumMember(Value = "Super_Soaker")]
    Super_Soaker,
    [EnumMember(Value = "Water_Spear")]
    Water_Spear,
    [EnumMember(Value = "Cloud")]
    Cloud,
    [EnumMember(Value = "Sunray")]
    Sunray,
    [EnumMember(Value = "Warm_Winds")]
    Warm_Winds,
    [EnumMember(Value = "Falling_Branch")]
    Falling_Branch, 
    [EnumMember(Value = "Sun_Shine")]
    Sun_Shine,
    [EnumMember(Value = "Sunny_Day")]
    Sunny_Day,
}

public static class ComponentMapper
{
    public static readonly Dictionary<CardComponentType , Type>
        CardComponents = new Dictionary<CardComponentType , Type>
    {
        { CardComponentType.Firebolt, typeof(Firebolt) },
        { CardComponentType.Embers, typeof(Embers) },
        { CardComponentType.Fire_Snake, typeof(Fire_Snake) },
        { CardComponentType.Water_Gun, typeof(Water_Gun) },
        { CardComponentType.Firewall, typeof(Firewall) },
        { CardComponentType.Flame_Robe, typeof(Flame_Robe) },
        { CardComponentType.Mist, typeof(Mist) },
        { CardComponentType.Super_Soaker, typeof(Super_Soaker) },
        { CardComponentType.Water_Spear, typeof(Water_Spear) },
        { CardComponentType.Cloud, typeof(Cloud) },
        { CardComponentType.Sunray, typeof(Sunray) },
        { CardComponentType.Warm_Winds, typeof(Warm_Winds) },
        { CardComponentType.Falling_Branch, typeof(Falling_Branch) },
        { CardComponentType.Sun_Shine, typeof(Sun_Shine) },
        { CardComponentType.Sunny_Day, typeof(Sunny_Day) },
        };
   
}