using RimGoddess.Base;
using RimGoddess.Equipment.Definition;
using UnityEngine;
using Verse;

namespace RimGoddess.Equipment;

public class TransformData
{
    public const string TRANSFORMED_PREFIX = "Transformed";

    private Color m_baseColor = Color.white;

    private Color m_hightlightColor = Color.white;

    private bool m_transformed;

    private TransformThingDef m_transformThingDef;

    public bool Transformed => m_transformed;

    public Color BaseColor => m_baseColor;

    public Color HighlightColor
    {
        get => m_hightlightColor;
        set => m_hightlightColor = value;
    }

    public void Make(ThingWithComps a_thing)
    {
        foreach (var allDef in DefDatabase<TransformThingDef>.AllDefs)
        {
            if (allDef.normalFormDef.defName != a_thing.def.defName &&
                allDef.transformedFormDef.defName != a_thing.def.defName)
            {
                continue;
            }

            m_transformThingDef = allDef;
            break;
        }

        Transform(null, a_thing, false, m_hightlightColor);
    }

    public void IOControl()
    {
        Scribe_Values.Look(ref m_transformed, "transformed");
        Scribe_Values.Look(ref m_baseColor, "baseColor");
        Scribe_Values.Look(ref m_hightlightColor, "highlightColor");
    }

    public void Transform(IGPawn a_pawn, ThingWithComps a_thing, bool a_value, Color a_baseColor)
    {
        m_baseColor = a_baseColor;
        m_transformed = a_value;
        if (m_transformThingDef == null)
        {
            Make(a_thing);
        }

        var thingDef = a_value ? m_transformThingDef?.transformedFormDef : m_transformThingDef?.normalFormDef;
        if (thingDef != null)
        {
            a_thing.def = thingDef;
            a_thing.Notify_ColorChanged();
        }
        else
        {
            Log.Error("RimGoddess - Equipment: Transformation def not found");
        }

        if (a_pawn == null)
        {
            return;
        }

        var compEquippable = a_thing.TryGetComp<CompEquippable>();
        if (compEquippable != null && a_pawn is Pawn pawn)
        {
            pawn.equipment.Notify_EquipmentRemoved(a_thing);
            compEquippable.verbTracker = new VerbTracker(compEquippable);
            _ = compEquippable.PrimaryVerb;
            pawn.equipment.Notify_EquipmentAdded(a_thing);
        }

        if (a_value)
        {
            a_pawn.AddAbility(m_transformThingDef?.goddessAbilityDef);
        }
        else
        {
            a_pawn.RemoveAbility(m_transformThingDef?.goddessAbilityDef);
        }
    }
}