using RimGoddess.Base;
using UnityEngine;
using Verse;

namespace RimGoddess.Equipment;

public class TransformThing : ThingWithComps, ITransformable
{
    private bool m_loaded;

    private IGPawn m_pawn;
    private TransformData m_transformData;

    public override Color DrawColor
    {
        get => m_transformData.HighlightColor;
        set => m_transformData.HighlightColor = value;
    }

    public override Color DrawColorTwo => m_transformData.BaseColor;

    public virtual void Notify_Transformed(IGPawn a_pawn, bool a_value, Color a_baseColor)
    {
        if (!m_loaded || m_transformData.Transformed == a_value)
        {
            return;
        }

        m_pawn = a_pawn;
        m_transformData.Transform(a_pawn, this, a_value, a_baseColor);
    }

    public void Notify_ShaderRefresh(Pawn a_pawn)
    {
    }

    public override void PostMake()
    {
        base.PostMake();
        if (m_loaded)
        {
            return;
        }

        m_loaded = true;
        m_transformData = new TransformData();
        m_transformData.Make(this);
    }

    public override void SpawnSetup(Map a_map, bool a_respawningAfterLoad)
    {
        if (!m_loaded)
        {
            m_loaded = true;
            m_transformData = new TransformData();
        }
        else if (!a_respawningAfterLoad && m_transformData.Transformed)
        {
            Notify_Transformed(m_pawn, false, m_transformData.BaseColor);
        }

        m_transformData.Make(this);
        base.SpawnSetup(a_map, a_respawningAfterLoad);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        if (!m_loaded)
        {
            m_transformData = new TransformData();
        }

        m_transformData.IOControl();
        if (Scribe.mode != LoadSaveMode.PostLoadInit)
        {
            return;
        }

        m_loaded = true;
        m_transformData.Transform(null, this, m_transformData.Transformed, m_transformData.HighlightColor);
    }
}