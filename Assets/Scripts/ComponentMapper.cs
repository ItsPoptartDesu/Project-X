using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardComponentType
{
    //Back
    Fell_Stinger, Infestation, Powder, Steamroller, Sticky_Web,//Bug
    Bite, Crunch, Feint_Attack, Pursuit, Thief,//Dark
    Breaking_Swipe, Clangorous_Soul, Dragon_Darts, Etenabeam, Max_Wyrmwind,//Dragon
    //Ears
    Attack_Order, Bug_Bite, Bug_Buzz, U_Turn, X_Scissors,//Bug
    Beat_Up, Flatter, Memento, Taunt, Torment,//Dark
    Devastating_Drake, Dragon_Dive, Dragon_Tail, Dual_Chop, Spacial_Rend,//Dragon
    //Eyes
    Fury_Cutter, Megahorn, Signal_Beam, Silver_Wind, Tail_Glow,//Bug
    Assurance, Fake_Tears, Knock_off, Payback, Snatch,//Dark
    Draco_Meteor, Dragon_Dance, Dragon_Pulse, Dragon_Rush, Roar_Of_Time,//Dragon
    //Forehead
    Leech_Life, Pin_Missile, Spider_Web, String_Shot, Twineedle,//Bug
    Dark_Pulse, Embargo, Fling, Punishment, Sucker_Punch,//Dark
    Dragon_Breath, Dragon_Claw, Dragon_Rage, Outrage, Twister,//Dragon
    //Mouth
    Defend_Order, Heal_Order, Quiver_Dance, Rage_Powder, Struggle_Bug,
    Dark_Void, Hone_Claws, NastyPlot, NightSlash, Switcheroo,
    Clanging_Scales, Clangorous_Soulblaze, Core_Enforcer, DragonHammer, Dynamax_Cannon,
    //Tail
    First_Impression, Lunge, Max_Flutterby, Pollen_Puff, Savage_Spin_Out,
    Foul_Play, Night_Daze, Parting_Shot, Quash, Snarl,
    Dragon_Berries, Dragon_Energy, Dragon_Pride, Dragon_Snot, Scale_Shot

}

public static class ComponentMapper
{
    public static readonly Dictionary<CardComponentType, Type>
        CardComponents = new Dictionary<CardComponentType, Type>
    {
            #region back
        { CardComponentType.Fell_Stinger, typeof(Fell_Stinger) },
        { CardComponentType.Infestation, typeof(Infestation) },
        { CardComponentType.Powder, typeof(Powder) },
        { CardComponentType.Steamroller, typeof(Steamroller) },
        { CardComponentType.Sticky_Web, typeof(Sticky_Web) },

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

