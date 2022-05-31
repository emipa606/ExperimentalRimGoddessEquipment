using System.Collections.Generic;
using RimGoddess.Base;
using Verse;

namespace RimGoddess.Equipment;

public class BlinkAbility : GoddessAbility
{
    public BlinkAbility(Pawn a_pawn)
        : base(a_pawn)
    {
    }

    public BlinkAbility(Pawn a_pawn, IGoddessAbilityDef a_def)
        : base(a_pawn, a_def)
    {
    }

    public override bool Activate(LocalTargetInfo a_currentTarget, LocalTargetInfo a_currentDestination)
    {
        var localTargetInfo = a_currentTarget;
        if (!localTargetInfo.IsValid)
        {
            return false;
        }

        var enumerable = GenRadial.RadialCellsAround(localTargetInfo.Cell, 2f, false);
        var list = new List<IntVec3>();
        foreach (var item in enumerable)
        {
            if (item.Standable(pawn.Map))
            {
                list.Add(item);
            }
        }

        if (!list.Any() || !Triggered())
        {
            return false;
        }

        var position = list.RandomElement();
        pawn.Position = position;
        pawn.Notify_Teleported();
        return true;
    }
}