using System.Collections.Generic;
using System.Linq;
using RimGoddess.Base;
using RimWorld;
using Verse;

namespace RimGoddess.Equipment;

public class HealingWatersAbility : GoddessAbility
{
    public HealingWatersAbility(Pawn a_pawn)
        : base(a_pawn)
    {
    }

    public HealingWatersAbility(Pawn a_pawn, IGoddessAbilityDef a_def)
        : base(a_pawn, a_def)
    {
    }

    private bool Cure(Hediff hediff)
    {
        if (!Triggered())
        {
            return false;
        }

        HealthUtility.Cure(hediff);
        Messages.Message("MessageHediffCuredByItem".Translate(hediff.LabelBase.CapitalizeFirst()), hediff.pawn,
            MessageTypeDefOf.PositiveEvent);
        return true;
    }

    private Hediff FindLifeThreateningHediff(Pawn a_targetPawn)
    {
        Hediff hediff = null;
        var num = -1f;
        var hediffs = a_targetPawn.health.hediffSet.hediffs;
        // ReSharper disable once ForCanBeConvertedToForeach, can be modified during iteration
        for (var i = 0; i < hediffs.Count; i++)
        {
            if (!hediffs[i].Visible || !hediffs[i].def.everCurableByItem || hediffs[i].FullyImmune())
            {
                continue;
            }

            var num2 = hediffs[i].CurStage?.lifeThreatening ?? false;
            if (!num2 && (!(hediffs[i].def.lethalSeverity >= 0f) ||
                          !(hediffs[i].Severity / hediffs[i].def.lethalSeverity >= 0.8f)))
            {
                continue;
            }

            var num3 = hediffs[i].Part != null ? hediffs[i].Part.coverageAbsWithChildren : 999f;
            if (hediff != null && !(num3 > num))
            {
                continue;
            }

            hediff = hediffs[i];
            num = num3;
        }

        return hediff;
    }

    private Hediff FindMostBleedingHediff(Pawn a_targetPawn)
    {
        var num = 0f;
        Hediff hediff = null;
        var hediffs = a_targetPawn.health.hediffSet.hediffs;
        // ReSharper disable once ForCanBeConvertedToForeach, can be modified during iteration
        for (var i = 0; i < hediffs.Count; i++)
        {
            if (!hediffs[i].Visible || !hediffs[i].def.everCurableByItem)
            {
                continue;
            }

            var bleedRate = hediffs[i].BleedRate;
            if (!(bleedRate > 0f) || !(bleedRate > num) && hediff != null)
            {
                continue;
            }

            num = bleedRate;
            hediff = hediffs[i];
        }

        return hediff;
    }

    private Hediff_Injury FindPermanentInjury(Pawn a_targetPawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
    {
        Hediff_Injury hediff_Injury = null;
        var hediffs = a_targetPawn.health.hediffSet.hediffs;
        // ReSharper disable once ForCanBeConvertedToForeach, can be modified during iteration
        for (var i = 0; i < hediffs.Count; i++)
        {
            if (hediffs[i] is Hediff_Injury { Visible: true } hediff_Injury2 && hediff_Injury2.IsPermanent() &&
                hediff_Injury2.def.everCurableByItem &&
                (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) &&
                (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
            {
                hediff_Injury = hediff_Injury2;
            }
        }

        return hediff_Injury;
    }

    private Hediff_Addiction FindAddiction(Pawn a_targetPawn)
    {
        var hediffs = a_targetPawn.health.hediffSet.hediffs;
        // ReSharper disable once ForCanBeConvertedToForeach, can be modified during iteration
        for (var i = 0; i < hediffs.Count; i++)
        {
            if (hediffs[i] is Hediff_Addiction { Visible: true } hediff_Addiction &&
                hediff_Addiction.def.everCurableByItem)
            {
                return hediff_Addiction;
            }
        }

        return null;
    }

    private Hediff_Injury FindInjury(Pawn a_targetPawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
    {
        Hediff_Injury hediff_Injury = null;
        var hediffs = a_targetPawn.health.hediffSet.hediffs;
        // ReSharper disable once ForCanBeConvertedToForeach, can be modified during iteration
        for (var i = 0; i < hediffs.Count; i++)
        {
            if (hediffs[i] is Hediff_Injury { Visible: true } hediff_Injury2 &&
                hediff_Injury2.def.everCurableByItem &&
                (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) &&
                (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
            {
                hediff_Injury = hediff_Injury2;
            }
        }

        return hediff_Injury;
    }

    public override bool Activate(LocalTargetInfo a_currentTarget, LocalTargetInfo a_currentDestination)
    {
        if (a_currentTarget.Pawn == null)
        {
            return false;
        }

        var currentTargetPawn = a_currentTarget.Pawn;
        var hediff = FindLifeThreateningHediff(currentTargetPawn);
        if (hediff != null)
        {
            return Cure(hediff);
        }

        if (HealthUtility.TicksUntilDeathDueToBloodLoss(currentTargetPawn) < 2500)
        {
            hediff = FindMostBleedingHediff(currentTargetPawn);
            if (hediff != null)
            {
                return Cure(hediff);
            }
        }

        if (currentTargetPawn.health.hediffSet.GetBrain() != null)
        {
            var hediff_Injury = FindPermanentInjury(currentTargetPawn,
                Gen.YieldSingle(currentTargetPawn.health.hediffSet.GetBrain()));
            if (hediff_Injury != null)
            {
                return Cure(hediff_Injury);
            }
        }

        var hediff_Addiction = FindAddiction(currentTargetPawn);
        if (hediff_Addiction != null)
        {
            return Cure(hediff_Addiction);
        }

        var hediff_Injury2 = FindPermanentInjury(currentTargetPawn);
        if (hediff_Injury2 != null)
        {
            return Cure(hediff_Injury2);
        }

        var hediff_Injury3 = FindInjury(currentTargetPawn);
        return hediff_Injury3 != null && Cure(hediff_Injury3);
    }
}