using System.Linq;
using RimWorld;
using Verse;

namespace RimGoddess.Equipment;

public class DivineStrike : OrbitalStrike
{
    public const int EffectiveRadius = 14;

    private static readonly SimpleCurve DistanceChanceFactor = new SimpleCurve
    {
        new CurvePoint(0f, 1f),
        new CurvePoint(15f, 0.1f)
    };

    public override void StartStrike()
    {
        base.StartStrike();
        MoteMaker.MakeBombardmentMote(Position, Map, 1f);
    }

    public override void Tick()
    {
        base.Tick();
        if (!Destroyed && Find.TickManager.TicksGame % 11 == 0)
        {
            CreateRandomExplosion();
        }
    }

    private void CreateRandomExplosion()
    {
        var enumerable = GenRadial.RadialCellsAround(Position, 5f, true);
        var intVec = Position;
        var num = enumerable.Count();
        var chance = 1f / num;
        foreach (var item in enumerable)
        {
            if (!item.InBounds(Map) || !Rand.Chance(chance))
            {
                continue;
            }

            intVec = item;
            break;
        }

        var num2 = Rand.Range(2f, 5f);
        GenExplosion.DoExplosion(intVec, Map, num2, DamageDefOf.Bomb, instigator, -1, -1f, null, projectile: def,
            weapon: weaponDef);
    }
}