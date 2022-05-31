using RimGoddess.Base;
using Verse;

namespace RimGoddess.Equipment;

public class SanctuaryAbility : GoddessAbility
{
    public SanctuaryAbility(Pawn a_pawn)
        : base(a_pawn)
    {
    }

    public SanctuaryAbility(Pawn a_pawn, IGoddessAbilityDef a_def)
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

        var thingDef = ThingDef.Named("BulletShieldPsychic");
        if (thingDef != null && Triggered())
        {
            GenSpawn.Spawn(thingDef, localTargetInfo.Cell, pawn.Map);
        }

        return false;
    }
}