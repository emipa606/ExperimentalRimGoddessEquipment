using System.Collections.Generic;
using RimGoddess.Equipment.Definition;
using Verse;

namespace RimGoddess.Equipment;

[StaticConstructorOnStartup]
public static class ItemInitializer
{
    static ItemInitializer()
    {
        var allDefsListForReading = DefDatabase<TransformThingDef>.AllDefsListForReading;
        var allDefsListForReading2 = DefDatabase<TetherThingDef>.AllDefsListForReading;
        if (RimGoddessEquipment.RaceMod == null)
        {
            Log.Message("RimGoddess - Equipment: Changing items costs to non race variants");
            foreach (var item in allDefsListForReading)
            {
                SetCostList(item.normalFormDef, item.nonRaceCostList);
            }

            {
                foreach (var item2 in allDefsListForReading2)
                {
                    SetCostList(item2.tetherThing, item2.nonRaceCostList);
                }

                return;
            }
        }

        foreach (var item3 in allDefsListForReading)
        {
            Log.Message(
                $"RimGoddess - Equipment: Transform Thing: {item3.defName}: {item3.normalFormDef.defName} -> {item3.transformedFormDef.defName}, {item3.goddessAbilityDefName}");
            if (item3.normalFormDef.descriptionHyperlinks == null)
            {
                item3.normalFormDef.descriptionHyperlinks = new List<DefHyperlink>();
            }

            if (item3.transformedFormDef.descriptionHyperlinks == null)
            {
                item3.transformedFormDef.descriptionHyperlinks = new List<DefHyperlink>();
            }

            var def = item3.goddessAbilityDef as Def;
            item3.normalFormDef.descriptionHyperlinks.Add(item3.transformedFormDef);
            item3.normalFormDef.descriptionHyperlinks.Add(def);
            item3.transformedFormDef.descriptionHyperlinks.Add(item3.normalFormDef);
            item3.transformedFormDef.descriptionHyperlinks.Add(def);
            if (item3.normalFormDef.IsApparel)
            {
                var normalFormDef = item3.normalFormDef;
                normalFormDef.description += "\n\n" + "RTN_Translation_GoddessApparelDescription".Translate();
            }
            else
            {
                var normalFormDef2 = item3.normalFormDef;
                normalFormDef2.description += "\n\n" + "RTN_Translation_GoddessEquipmentDescription".Translate();
            }
        }

        foreach (var item4 in allDefsListForReading2)
        {
            Log.Message(
                $"RimGoddess - Equipment: Tether Thing: {item4.defName}: {item4.tetherThing.defName} -> {item4.tetheredThing.defName}");
            if (item4.tetherThing.descriptionHyperlinks == null)
            {
                item4.tetherThing.descriptionHyperlinks = new List<DefHyperlink>();
            }

            if (item4.tetheredThing.descriptionHyperlinks == null)
            {
                item4.tetheredThing.descriptionHyperlinks = new List<DefHyperlink>();
            }

            item4.tetherThing.descriptionHyperlinks.Add(item4.tetheredThing);
            item4.tetheredThing.descriptionHyperlinks.Add(item4.tetherThing);
        }
    }

    private static void SetCostList(ThingDef a_thing, List<ThingDefCountClass> a_costList)
    {
        a_thing.costList = a_costList;
        var named = DefDatabase<RecipeDef>.GetNamed("Make_" + a_thing.defName);
        if (named == null)
        {
            return;
        }

        named.ingredients.Clear();
        foreach (var a_cost in a_costList)
        {
            var ingredientCount = new IngredientCount();
            ingredientCount.SetBaseCount(a_cost.count);
            ingredientCount.filter.SetAllow(a_cost.thingDef, true);
            named.ingredients.Add(ingredientCount);
        }
    }
}