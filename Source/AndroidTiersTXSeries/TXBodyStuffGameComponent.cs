using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AndroidTiersTXSeries;

public class TXBodyStuffGameComponent : GameComponent
{
    public Dictionary<string, BeardDef> OriginalBeards;
    private List<string> originalBeardsKeys;
    private List<BeardDef> originalBeardsValues;

    public Dictionary<string, TattooDef> OriginalBodyTattoos;
    private List<string> originalBodyTattoosKeys;
    private List<TattooDef> originalBodyTattoosValues;
    public Dictionary<string, TattooDef> OriginalFaceTattoos;
    private List<string> originalFaceTattoosKeys;
    private List<TattooDef> originalFaceTattoosValues;

    public Dictionary<string, HairDef> OriginalHairs;
    private List<string> originalHairsKeys;
    private List<HairDef> originalHairsValues;

    public TXBodyStuffGameComponent(Game game)
    {
        AndroidTiersTXSeries.TXBodyStuff = this;
    }


    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref OriginalHairs, "OriginalHairs", LookMode.Value,
            LookMode.Def,
            ref originalHairsKeys, ref originalHairsValues);
        Scribe_Collections.Look(ref OriginalBeards, "OriginalBeards", LookMode.Value,
            LookMode.Def,
            ref originalBeardsKeys, ref originalBeardsValues);
        Scribe_Collections.Look(ref OriginalBodyTattoos, "OriginalBodyTattoos", LookMode.Value,
            LookMode.Def,
            ref originalBodyTattoosKeys, ref originalBodyTattoosValues);
        Scribe_Collections.Look(ref OriginalFaceTattoos, "OriginalFaceTattoos", LookMode.Value,
            LookMode.Def,
            ref originalFaceTattoosKeys, ref originalFaceTattoosValues);
    }

    public override void StartedNewGame()
    {
        base.StartedNewGame();
        Initiate();
    }

    private void Initiate()
    {
        OriginalBeards = new Dictionary<string, BeardDef>();
        OriginalHairs = new Dictionary<string, HairDef>();
        OriginalBodyTattoos = new Dictionary<string, TattooDef>();
        OriginalFaceTattoos = new Dictionary<string, TattooDef>();
    }
}