using Verse;

namespace RimGoddess.Equipment;

public class RimGoddessEquipment : Mod
{
    public static RimGoddessEquipment Instance;

    public static ModContentPack RaceMod;

    public RimGoddessEquipment(ModContentPack a_modContentPack)
        : base(a_modContentPack)
    {
        Instance = this;
        RaceMod = null;
        foreach (var runningMod in LoadedModManager.RunningMods)
        {
            switch (runningMod.PackageId.ToLower())
            {
                case "mlie.experimentalrimgoddessrace":
                case "bladeofdebt.rimgoddess.raceex":
                case "bladeofdebt.rimgoddess.race":
                case "bladeofdebt.rimgodess.race[test]":
                    RaceMod = runningMod;
                    Log.Message("RimGoddess - Equipment: RimGoddess - Race found, using race defs");
                    goto end_IL_006d;
            }

            continue;
            end_IL_006d:
            break;
        }

        if (RaceMod == null)
        {
            Log.Message("RimGoddess - Equipment: RimGoddess - Race not found, using standalone mode");
        }

        Log.Message("RimGoddess - Equipment: Loaded");
    }
}