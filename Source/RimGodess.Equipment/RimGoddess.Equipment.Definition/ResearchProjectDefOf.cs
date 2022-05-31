using RimWorld;
using Verse;

namespace RimGoddess.Equipment.Definition;

[DefOf]
public static class ResearchProjectDefOf
{
    public static ResearchProjectDef RTN_ResearchProject_WeaponCrafting;

    public static ResearchProjectDef RTN_ResearchProject_ApparelCrafting;

    public static ResearchProjectDef RTN_ResearchProject_FaithInfusion;

    static ResearchProjectDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
    }
}