using System;
using System.Collections.Generic;
using System.Reflection;
using RimGoddess.Base;
using Verse;

namespace RimGoddess.Equipment.Definition;

public class TransformThingDef : Def
{
    public IGoddessAbilityDef goddessAbilityDef;

    public string goddessAbilityDefName;
    public List<ThingDefCountClass> nonRaceCostList;

    public ThingDef normalFormDef;

    public ThingDef transformedFormDef;

    public override void PostLoad()
    {
        if (RimGoddessEquipment.RaceMod == null || string.IsNullOrEmpty(goddessAbilityDefName))
        {
            return;
        }

        LongEventHandler.ExecuteWhenFinished(delegate
        {
            Log.Message("RimGoddess - Equipment: Attempting to load RimGoddess - Race ability");
            var typeFromHandle = typeof(DefDatabase<>);
            var type = Type.GetType("RimGoddess.Race.GoddessAbilityDef, RimGodess.Race");
            if (type == null)
            {
                return;
            }

            var method = typeFromHandle.MakeGenericType(type)
                .GetMethod("GetNamed", BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                return;
            }

            goddessAbilityDef =
                method.Invoke(null, new object[] { goddessAbilityDefName, true }) as IGoddessAbilityDef;
            Log.Message($"RimGoddess - Equipment: Loaded RimGoddess - Race ability : {goddessAbilityDef?.Label}");
        });
    }

    public override IEnumerable<string> ConfigErrors()
    {
        foreach (var item in base.ConfigErrors())
        {
            yield return item;
        }

        if (normalFormDef == null)
        {
            yield return "normalFormDef == null";
        }

        if (transformedFormDef == null)
        {
            yield return "transformedFormDef == null";
        }
    }
}