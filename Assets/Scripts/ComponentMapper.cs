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
    [EnumMember(Value = "Water_Gun")]
    Water_Gun,
    [EnumMember(Value = "Firewall")]
    Firewall,
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
        };
   
}