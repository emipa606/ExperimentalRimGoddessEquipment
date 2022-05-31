using RimGoddess.Equipment.Definition;
using Verse;

namespace RimGoddess.Equipment;

public class TetherData
{
    private bool m_tethered;
    private TetherThingDef m_tetherThingDef;

    public bool Tethered => m_tethered;

    public float TetherFaithDraw => m_tetherThingDef.tetherFaithDraw;

    public void Make(ThingWithComps a_thing)
    {
        foreach (var allDef in DefDatabase<TetherThingDef>.AllDefs)
        {
            if (allDef.tetherThing.defName != a_thing.def.defName &&
                allDef.tetheredThing.defName != a_thing.def.defName)
            {
                continue;
            }

            m_tetherThingDef = allDef;
            break;
        }
    }

    public void IOControl()
    {
        Scribe_Values.Look(ref m_tethered, "tethered");
    }

    public void Tether(Pawn a_pawn, ThingWithComps a_thing, bool a_value)
    {
        m_tethered = a_value;
        if (m_tetherThingDef == null)
        {
            Make(a_thing);
        }

        var thingDef = a_value ? m_tetherThingDef?.tetheredThing : m_tetherThingDef?.tetherThing;
        if (thingDef != null)
        {
            a_thing.def = thingDef;
            a_thing.Notify_ColorChanged();
        }
        else
        {
            Log.Error("RimGoddess - Equipment: Tether def not found");
        }

        if (a_pawn == null)
        {
            return;
        }

        var compEquippable = a_thing.TryGetComp<CompEquippable>();
        if (compEquippable == null)
        {
            return;
        }

        a_pawn.equipment.Notify_EquipmentRemoved(a_thing);
        compEquippable.verbTracker = new VerbTracker(compEquippable);
        _ = compEquippable.PrimaryVerb;
        a_pawn.equipment.Notify_EquipmentAdded(a_thing);
    }
}