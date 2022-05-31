using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimGoddess.Equipment;

[StaticConstructorOnStartup]
public class Tunica : TransformApparel
{
    private const float ChargingTime = 1.5f;

    public const float MaxCharge = 125f;

    public const float DisappearTime = 20f;

    private static readonly Material BubbleMat =
        MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent, Color.cyan);

    public float Charge { get; private set; }

    public float TimePassed { get; private set; } = 20f;

    public override void Tick()
    {
        base.Tick();
        if (TimePassed >= 20f)
        {
            Charge = 0f;
            return;
        }

        if (TimePassed <= 1.5f)
        {
            Charge = Mathf.Min(Charge + 1.388889f, 125f);
        }

        TimePassed += 1f / 60f;
    }

    public override bool CheckPreAbsorbDamage(DamageInfo a_dinfo)
    {
        if (TimePassed >= 20f || Charge <= 0f)
        {
            return false;
        }

        Charge -= a_dinfo.Amount;
        return true;
    }

    public override IEnumerable<Gizmo> GetWornGizmos()
    {
        if (!(TimePassed < 20f))
        {
            yield break;
        }

        var gizmoDivineShieldStatus = new GizmoDivineShieldStatus
        {
            Tunica = this
        };
        yield return gizmoDivineShieldStatus;
    }

    public void ResetCharge()
    {
        TimePassed = 0f;
        Charge = 0f;
    }

    public override void DrawWornExtras()
    {
        if (!(Charge > 0f) || !(TimePassed < 20f))
        {
            return;
        }

        var num = Mathf.Lerp(1.2f, 2.15f, Mathf.Min(Charge / 125f, 1f - (TimePassed / 20f)));
        var drawPos = Wearer.Drawer.DrawPos;
        drawPos.y = AltitudeLayer.Blueprint.AltitudeFor();
        float angle = Rand.Range(0, 360);
        var s = new Vector3(num, 1f, num);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
        Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
    }
}