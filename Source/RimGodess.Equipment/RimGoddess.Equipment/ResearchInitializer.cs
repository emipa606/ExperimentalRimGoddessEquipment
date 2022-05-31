using System.Collections.Generic;
using RimGoddess.Equipment.Definition;
using Verse;

namespace RimGoddess.Equipment;

[StaticConstructorOnStartup]
public static class ResearchInitializer
{
    static ResearchInitializer()
    {
        var named = DefDatabase<ResearchProjectDef>.GetNamed("RTN_ResearchProject_GoddessBasics", false);
        if (named == null)
        {
            return;
        }

        var rTN_ResearchProject_FaithInfusion = ResearchProjectDefOf.RTN_ResearchProject_FaithInfusion;
        if (rTN_ResearchProject_FaithInfusion.prerequisites == null)
        {
            rTN_ResearchProject_FaithInfusion.prerequisites = new List<ResearchProjectDef>();
        }

        rTN_ResearchProject_FaithInfusion.prerequisites.Add(named);
    }
}