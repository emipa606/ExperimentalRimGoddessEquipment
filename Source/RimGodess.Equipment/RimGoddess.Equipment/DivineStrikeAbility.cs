using RimGoddess.Base;
using RimGoddess.Equipment.Definition;
using Verse;

namespace RimGoddess.Equipment;

public class DivineStrikeAbility : GoddessAbility
{
    public DivineStrikeAbility(Pawn a_pawn)
        : base(a_pawn)
    {
    }

    public DivineStrikeAbility(Pawn a_pawn, IGoddessAbilityDef a_def)
        : base(a_pawn, a_def)
    {
    }

    public override bool Activate(LocalTargetInfo a_currentTarget, LocalTargetInfo a_currentDestination)
    {
        if (a_currentTarget.HasThing && a_currentTarget.Thing.Map != pawn.Map)
        {
            return false;
        }

        if (!Triggered())
        {
            return false;
        }

        if (GenSpawn.Spawn(ThingDefOf.RTN_Thing_DivineStrike, a_currentTarget.Cell, pawn.Map) is not DivineStrike obj)
        {
            return true;
        }

        obj.duration = 25;
        obj.instigator = pawn;
        obj.weaponDef = null;
        obj.StartStrike();

        return true;
    }
}