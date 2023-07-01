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
    [EnumMember(Value = "Dragon_Snot")]
    Dragon_Snot,
    [EnumMember(Value = "Megahorn")]
    Megahorn,
    [EnumMember(Value = "Spider_Web")]
    Spider_Web,
    [EnumMember(Value = "Firebolt")]
    Firebolt,
    [EnumMember(Value = "Switcheroo")]
    Switcheroo,
    [EnumMember(Value = "Firewall")]
    Firewall,
}

public static class ComponentMapper
{
    public static readonly Dictionary<CardComponentType , Type>
        CardComponents = new Dictionary<CardComponentType , Type>
    {
        { CardComponentType.Firebolt, typeof(Firebolt) },
        { CardComponentType.Megahorn, typeof(Megahorn) },
        { CardComponentType.Spider_Web, typeof(Spider_Web) },
        { CardComponentType.Switcheroo, typeof(Switcheroo) },
        { CardComponentType.Firewall, typeof(Firewall) },
        { CardComponentType.Dragon_Snot, typeof(Dragon_Snot) },
        };
}