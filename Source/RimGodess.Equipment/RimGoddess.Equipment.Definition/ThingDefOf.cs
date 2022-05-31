using RimWorld;
using Verse;

namespace RimGoddess.Equipment.Definition;

[DefOf]
public static class ThingDefOf
{
    public static ThingDef RTN_Thing_DivineStrike;

    static ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}