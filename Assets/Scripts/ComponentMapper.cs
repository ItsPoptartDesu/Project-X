using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

public enum CardComponentType
{
    [EnumMember(Value = "Assurance")]
    Assurance,
    [EnumMember(Value = "Attack_Order")]
    Attack_Order,
    [EnumMember(Value = "Beat_Up")]
    Beat_Up,
    [EnumMember(Value = "Bug_Bite")]
    Bug_Bite,
    [EnumMember(Value = "Bite")]
    Bite,
    [EnumMember(Value = "BODY")]
    BODY,
    [EnumMember(Value = "Breaking_Swipe")]
    Breaking_Swipe,
    [EnumMember(Value = "Bug_Buzz")]
    Bug_Buzz,
    [EnumMember(Value = "Clanging_Scales")]
    Clanging_Scales,
    [EnumMember(Value = "Clangorous_Soul")]
    Clangorous_Soul,
    [EnumMember(Value = "Clangorous_Soulblaze")]
    Clangorous_Soulblaze,
    [EnumMember(Value = "Core_Enforcer")]
    Core_Enforcer,
    [EnumMember(Value = "Crunch")]
    Crunch,
    [EnumMember(Value = "Dark_Pulse")]
    Dark_Pulse,
    [EnumMember(Value = "Dark_Void")]
    Dark_Void,
    [EnumMember(Value = "Defend_Order")]
    Defend_Order,
    [EnumMember(Value = "Devastating_Drake")]
    Devastating_Drake,
    [EnumMember(Value = "Draco_Meteor")]
    Draco_Meteor,
    [EnumMember(Value = "DragonHammer")]
    DragonHammer,
    [EnumMember(Value = "Dragon_Berries")]
    Dragon_Berries,
    [EnumMember(Value = "Dragon_Breath")]
    Dragon_Breath,
    [EnumMember(Value = "Dragon_Claw")]
    Dragon_Claw,
    [EnumMember(Value = "Dragon_Dance")]
    Dragon_Dance,
    [EnumMember(Value = "Dragon_Darts")]
    Dragon_Darts,
    [EnumMember(Value = "Dragon_Dive")]
    Dragon_Dive,
    [EnumMember(Value = "Dragon_Energy")]
    Dragon_Energy,
    [EnumMember(Value = "Dragon_Pride")]
    Dragon_Pride,
    [EnumMember(Value = "Dragon_Pulse")]
    Dragon_Pulse,
    [EnumMember(Value = "Dragon_Rage")]
    Dragon_Rage,
    [EnumMember(Value = "Dragon_Rush")]
    Dragon_Rush,
    [EnumMember(Value = "Dragon_Snot")]
    Dragon_Snot,
    [EnumMember(Value = "Dragon_Tail")]
    Dragon_Tail,
    [EnumMember(Value = "Dual_Chop")]
    Dual_Chop,
    [EnumMember(Value = "Dynamax_Cannon")]
    Dynamax_Cannon,
    [EnumMember(Value = "Embargo")]
    Embargo, 
    [EnumMember(Value = "Etenabeam")]
    Etenabeam, 
    [EnumMember(Value = "Fake_Tears")]
    Fake_Tears,
    [EnumMember(Value = "Feint_Attack")]
    Feint_Attack,
    [EnumMember(Value = "Bone_Sail")]
    Bone_Sail,
    [EnumMember(Value = "First_Impression")]
    First_Impression,
    [EnumMember(Value = "Flatter")]
    Flatter,
    [EnumMember(Value = "Fling")]
    Fling,
    [EnumMember(Value = "Foul_Play")]
    Foul_Play, 
    [EnumMember(Value = "Fury_Cutter")]
    Fury_Cutter,
    [EnumMember(Value = "Heal_Order")]
    Heal_Order,
    [EnumMember(Value = "Hone_Claws")]
    Hone_Claws, 
    [EnumMember(Value = "Croc")]
    Croc,
    [EnumMember(Value = "Knock_off")]
    Knock_off, 
    [EnumMember(Value = "Leech_Life")]
    Leech_Life,
    [EnumMember(Value = "Lunge")]
    Lunge,
    [EnumMember(Value = "Max_Flutterby")]
    Max_Flutterby, 
    [EnumMember(Value = "Max_Wyrmwind")]
    Max_Wyrmwind,
    [EnumMember(Value = "Megahorn")]
    Megahorn, 
    [EnumMember(Value = "Memento")]
    Memento,
    [EnumMember(Value = "NastyPlot")]
    NastyPlot, 
    [EnumMember(Value = "NightSlash")]
    NightSlash, 
    [EnumMember(Value = "Night_Daze")]
    Night_Daze,
    [EnumMember(Value = "Outrage")]
    Outrage, 
    [EnumMember(Value = "Parting_Shot")]
    Parting_Shot,
    [EnumMember(Value = "Payback")]
    Payback,
    [EnumMember(Value = "Pin_Missile")]
    Pin_Missile,
    [EnumMember(Value = "Pollen_Puff")]
    Pollen_Puff, 
    [EnumMember(Value = "Green_Thorns")]
    Green_Thorns, 
    [EnumMember(Value = "Punishment")]
    Punishment, 
    [EnumMember(Value = "Pursuit")]
    Pursuit,
    [EnumMember(Value = "Quash")]
    Quash, 
    [EnumMember(Value = "Quiver_Dance")]
    Quiver_Dance,
    [EnumMember(Value = "Rage_Powder")]
    Rage_Powder, 
    [EnumMember(Value = "Roar_Of_Time")]
    Roar_Of_Time,
    [EnumMember(Value = "Savage_Spin_Out")]
    Savage_Spin_Out,
    [EnumMember(Value = "Scale_Shot")]
    Scale_Shot, 
    [EnumMember(Value = "Signal_Beam")]
    Signal_Beam,
    [EnumMember(Value = "Silver_Wind")]
    Silver_Wind,
    [EnumMember(Value = "Snarl")]
    Snarl, 
    [EnumMember(Value = "Snatch")]
    Snatch, 
    [EnumMember(Value = "Spacial_Rend")]
    Spacial_Rend,
    [EnumMember(Value = "Spider_Web")]
    Spider_Web,
    [EnumMember(Value = "Indian_Star")]
    Indian_Star, 
    [EnumMember(Value = "Red_Ear")]
    Red_Ear,
    [EnumMember(Value = "String_Shot")]
    String_Shot, 
    [EnumMember(Value = "Struggle_Bug")]
    Struggle_Bug, 
    [EnumMember(Value = "Sucker_Punch")]
    Sucker_Punch,
    [EnumMember(Value = "Switcheroo")]
    Switcheroo,
    [EnumMember(Value = "Tail_Glow")]
    Tail_Glow, 
    [EnumMember(Value = "Taunt")]
    Taunt,
    [EnumMember(Value = "Thief")]
    Thief, 
    [EnumMember(Value = "Torment")]
    Torment, 
    [EnumMember(Value = "Twineedle")]
    Twineedle,
    [EnumMember(Value = "Twister")]
    Twister,
    [EnumMember(Value = "U_Turn")]
    U_Turn, 
    [EnumMember(Value = "X_Scissors")]
    X_Scissors,
    [EnumMember(Value = "Tiny_Dino")]
    Tiny_Dino,
    [EnumMember(Value = "Tri_Spikes")]
    Tri_Spikes,
}

public static class ComponentMapper
{
    public static readonly Dictionary<CardComponentType , Type>
        CardComponents = new Dictionary<CardComponentType , Type>
    {
            #region back
        { CardComponentType.Bone_Sail, typeof(Bone_Sail) },
        { CardComponentType.Croc, typeof(Croc) },
        { CardComponentType.Green_Thorns, typeof(Green_Thorns) },
        { CardComponentType.Indian_Star, typeof(Indian_Star) },
        { CardComponentType.Red_Ear, typeof(Red_Ear) },
        { CardComponentType.Tiny_Dino, typeof(Tiny_Dino) },
        { CardComponentType.Tri_Spikes, typeof(Tri_Spikes) },


        { CardComponentType.Bite, typeof(Bite) },
        { CardComponentType.Crunch, typeof(Crunch) },
        { CardComponentType.Feint_Attack, typeof(Feint_Attack) },
        { CardComponentType.Pursuit, typeof(Pursuit) },
        { CardComponentType.Thief, typeof(Thief) },

        { CardComponentType.Breaking_Swipe, typeof(Breaking_Swipe) },
        { CardComponentType.Clangorous_Soul, typeof(Clangorous_Soul) },
        { CardComponentType.Dragon_Darts, typeof(Dragon_Darts) },
        { CardComponentType.Etenabeam, typeof(Etenabeam) },
        { CardComponentType.Max_Wyrmwind, typeof(Max_Wyrmwind) },
            #endregion
            #region Ears
        { CardComponentType.Attack_Order, typeof(Attack_Order) },
        { CardComponentType.Bug_Bite, typeof(Bug_Bite) },
        { CardComponentType.Bug_Buzz, typeof(Bug_Buzz) },
        { CardComponentType.U_Turn, typeof(U_Turn) },
        { CardComponentType.X_Scissors, typeof(X_Scissors) },

        { CardComponentType.Beat_Up, typeof(Beat_Up) },
        { CardComponentType.Flatter, typeof(Flatter) },
        { CardComponentType.Memento, typeof(Memento) },
        { CardComponentType.Taunt, typeof(Taunt) },
        { CardComponentType.Torment, typeof(Torment) },

        { CardComponentType.Devastating_Drake, typeof(Devastating_Drake) },
        { CardComponentType.Dragon_Dive, typeof(Dragon_Dive) },
        { CardComponentType.Dragon_Tail, typeof(Dragon_Tail) },
        { CardComponentType.Dual_Chop, typeof(Dual_Chop) },
        { CardComponentType.Spacial_Rend, typeof(Spacial_Rend) },
            #endregion
            #region Eyes
        { CardComponentType.Fury_Cutter, typeof(Fury_Cutter) },
        { CardComponentType.Megahorn, typeof(Megahorn) },
        { CardComponentType.Signal_Beam, typeof(Signal_Beam) },
        { CardComponentType.Silver_Wind, typeof(Silver_Wind) },
        { CardComponentType.Tail_Glow, typeof(Tail_Glow) },

        { CardComponentType.Assurance, typeof(Assurance) },
        { CardComponentType.Fake_Tears, typeof(Fake_Tears) },
        { CardComponentType.Knock_off, typeof(Knock_off) },
        { CardComponentType.Payback, typeof(Payback) },
        { CardComponentType.Snatch, typeof(Snatch) },

        { CardComponentType.Draco_Meteor, typeof(Draco_Meteor) },
        { CardComponentType.Dragon_Dance, typeof(Dragon_Dance) },
        { CardComponentType.Dragon_Pulse, typeof(Dragon_Pulse) },
        { CardComponentType.Dragon_Rush, typeof(Dragon_Rush) },
        { CardComponentType.Roar_Of_Time, typeof(Roar_Of_Time) },
            #endregion
            #region Forehead
        { CardComponentType.Leech_Life, typeof(Leech_Life) },
        { CardComponentType.Pin_Missile, typeof(Pin_Missile) },
        { CardComponentType.Spider_Web, typeof(Spider_Web) },
        { CardComponentType.String_Shot, typeof(String_Shot) },
        { CardComponentType.Twineedle, typeof(Twineedle) },

        { CardComponentType.Dark_Pulse, typeof(Dark_Pulse) },
        { CardComponentType.Embargo, typeof(Embargo) },
        { CardComponentType.Fling, typeof(Fling) },
        { CardComponentType.Punishment, typeof(Punishment) },
        { CardComponentType.Sucker_Punch, typeof(Sucker_Punch) },

        { CardComponentType.Dragon_Breath, typeof(Dragon_Breath) },
        { CardComponentType.Dragon_Claw, typeof(Dragon_Claw) },
        { CardComponentType.Dragon_Rage, typeof(Dragon_Rage) },
        { CardComponentType.Outrage, typeof(Outrage) },
        { CardComponentType.Twister, typeof(Twister) },
            #endregion
            #region Mouth
        { CardComponentType.Defend_Order, typeof(Defend_Order) },
        { CardComponentType.Heal_Order, typeof(Heal_Order) },
        { CardComponentType.Quiver_Dance, typeof(Quiver_Dance) },
        { CardComponentType.Rage_Powder, typeof(Rage_Powder) },
        { CardComponentType.Struggle_Bug, typeof(Struggle_Bug) },

        { CardComponentType.Dark_Void, typeof(Dark_Void) },
        { CardComponentType.Hone_Claws, typeof(Hone_Claws) },
        { CardComponentType.NastyPlot, typeof(NastyPlot) },
        { CardComponentType.NightSlash, typeof(NightSlash) },
        { CardComponentType.Switcheroo, typeof(Switcheroo) },

        { CardComponentType.Clanging_Scales, typeof(Clanging_Scales) },
        { CardComponentType.Clangorous_Soulblaze, typeof(Clangorous_Soulblaze) },
        { CardComponentType.Core_Enforcer, typeof(Core_Enforcer) },
        { CardComponentType.DragonHammer, typeof(DragonHammer) },
        { CardComponentType.Dynamax_Cannon, typeof(Dynamax_Cannon) },
            #endregion
            #region Tail
        { CardComponentType.First_Impression, typeof(First_Impression) },
        { CardComponentType.Lunge, typeof(Lunge) },
        { CardComponentType.Max_Flutterby, typeof(Max_Flutterby) },
        { CardComponentType.Pollen_Puff, typeof(Pollen_Puff) },
        { CardComponentType.Savage_Spin_Out, typeof(Savage_Spin_Out) },

        { CardComponentType.Foul_Play, typeof(Foul_Play) },
        { CardComponentType.Night_Daze, typeof(Night_Daze) },
        { CardComponentType.Parting_Shot, typeof(Parting_Shot) },
        { CardComponentType.Quash, typeof(Quash) },
        { CardComponentType.Snarl, typeof(Snarl) },

        { CardComponentType.Dragon_Berries, typeof(Dragon_Berries) },
        { CardComponentType.Dragon_Energy, typeof(Dragon_Energy) },
        { CardComponentType.Dragon_Pride, typeof(Dragon_Pride) },
        { CardComponentType.Dragon_Snot, typeof(Dragon_Snot) },
        { CardComponentType.Scale_Shot, typeof(Scale_Shot) },
            #endregion
        };
}