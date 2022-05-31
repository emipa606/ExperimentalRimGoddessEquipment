using RimGoddess.Base;
using Verse;

namespace RimGoddess.Equipment;

public class DivineShieldAbility : GoddessAbility
{
    public DivineShieldAbility(Pawn a_pawn)
        : base(a_pawn)
    {
    }

    public DivineShieldAbility(Pawn a_pawn, IGoddessAbilityDef a_def)
        : base(a_pawn, a_def)
    {
    }

    public override bool Activate(LocalTargetInfo a_currentTarget, LocalTargetInfo a_currentDestination)
    {
        var currentTargetPawn = a_currentTarget.Pawn;
        if (currentTargetPawn == null)
        {
            return false;
        }

        foreach (var item in currentTargetPawn.apparel.WornApparel)
        {
            if (item is not Tunica tunica)
            {
                continue;
            }

            tunica.ResetCharge();
            return true;
        }

        return false;
    }
}