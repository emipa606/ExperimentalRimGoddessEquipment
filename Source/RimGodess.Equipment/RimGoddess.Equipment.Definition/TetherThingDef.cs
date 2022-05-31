using System.Collections.Generic;
using Verse;

namespace RimGoddess.Equipment.Definition;

internal class TetherThingDef : Def
{
    public List<ThingDefCountClass> nonRaceCostList;

    public ThingDef tetheredThing;

    public float tetherFaithDraw;

    public ThingDef tetherThing;
}