using RimGoddess.Base;
using RimWorld;
using Verse;

namespace RimGoddess.Equipment;

public class TetherApparel : Apparel, ITetherEquipment
{
    private bool m_loaded;
    private TetherData m_tetherData;

    public bool Tethered => m_tetherData.Tethered;

    public float TetherFaithDraw => m_tetherData.TetherFaithDraw;

    public void Notify_Tethered(Pawn a_pawn, bool a_value)
    {
        m_tetherData.Tether(a_pawn, this, a_value);
    }

    public override void PostMake()
    {
        base.PostMake();
        if (m_loaded)
        {
            return;
        }

        m_loaded = true;
        m_tetherData = new TetherData();
        m_tetherData.Make(this);
    }

    public override void SpawnSetup(Map a_map, bool a_respawningAfterLoad)
    {
        if (!m_loaded)
        {
            m_tetherData = new TetherData();
            m_tetherData.Make(this);
        }

        m_tetherData.Make(this);
        base.SpawnSetup(a_map, a_respawningAfterLoad);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        if (!m_loaded)
        {
            m_tetherData = new TetherData();
        }

        m_tetherData.IOControl();
        if (Scribe.mode != LoadSaveMode.PostLoadInit)
        {
            return;
        }

        m_loaded = true;
        m_tetherData.Tether(null, this, m_tetherData.Tethered);
    }
}