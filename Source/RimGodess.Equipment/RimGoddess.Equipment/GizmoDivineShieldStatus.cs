using UnityEngine;
using Verse;

namespace RimGoddess.Equipment;

[StaticConstructorOnStartup]
public class GizmoDivineShieldStatus : Gizmo
{
    private static readonly Texture2D FullShieldBarTex =
        SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

    private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

    public GizmoDivineShieldStatus()
    {
        base.Order = -120f;
    }

    public Tunica Tunica { get; set; }

    public override float GetWidth(float a_maxWidth)
    {
        return 140f;
    }

    public override GizmoResult GizmoOnGUI(Vector2 a_topLeft, float a_maxWidth, GizmoRenderParms parms)
    {
        var rect = new Rect(a_topLeft.x, a_topLeft.y, GetWidth(a_maxWidth), 75f);
        Widgets.DrawWindowBackground(rect);
        var charge = Tunica.Charge;
        var rect2 = rect.ContractedBy(6f);
        var rect3 = rect2;
        rect3.height = rect.height / 2f;
        Text.Font = GameFont.Tiny;
        Widgets.Label(rect3,
            $"{"RTN_Translation_TimeLeft".Translate()}: {20f - Tunica.TimePassed}");
        var rect4 = rect2;
        rect4.yMin = rect2.y + (rect2.height / 2f);
        var fillPercent = charge / 125f;
        Widgets.FillableBar(rect4, fillPercent, FullShieldBarTex, EmptyShieldBarTex, false);
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect4, $"{charge} / {125f}");
        Text.Anchor = TextAnchor.UpperLeft;
        return new GizmoResult(GizmoState.Clear);
    }
}