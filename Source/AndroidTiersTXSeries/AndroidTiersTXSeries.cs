using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AndroidTiersTXSeries;

[StaticConstructorOnStartup]
public static class AndroidTiersTXSeries
{
    static AndroidTiersTXSeries()
    {
        new Harmony("Mlie.AndroidTiersTXSeries").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static void UpdatePawnGraphics(ref Pawn pawn)
    {
        if (!pawn.def.defName.StartsWith("ATPP_"))
        {
            return;
        }

        if (pawn.def.defName.EndsWith("ITX"))
        {
            return;
        }

        var female = pawn.story.bodyType.defName.Contains("Female");

        var type = 0;

        switch (pawn.health.summaryHealth.SummaryHealthPercent)
        {
            case <= 0.85f and > 0.45f:
                type = 1;
                break;
            case <= 0.45f:
                type = 2;
                break;
        }

        var bodyToSet = female ? BodyTypeDefOf.Female : BodyTypeDefOf.Male;
        var headToSet = HeadTypeDefOf.Skull;

        if (pawn.def == AndroidTiersTXSeriesRaces_DefOf.ATPP_Android2TX)
        {
            switch (type)
            {
                case 1:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted12TX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted12TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX2_Female_Average_Hurted
                        : AndroidTiersTXSeriesHeads_DefOf.TX2_Male_Average_Hurted;
                    break;
                case 2:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted22TX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted22TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX2_Female_Average_Hurted2
                        : AndroidTiersTXSeriesHeads_DefOf.TX2_Male_Average_Hurted2;
                    break;
                default:
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX2_Female_Average_Normal
                        : AndroidTiersTXSeriesHeads_DefOf.TX2_Male_Average_Normal;
                    break;
            }
        }

        if (pawn.def == AndroidTiersTXSeriesRaces_DefOf.ATPP_Android2KTX)
        {
            switch (type)
            {
                case 1:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted12KTX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted12KTX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX2K_Female_Average_Hurted
                        : AndroidTiersTXSeriesHeads_DefOf.TX2K_Male_Average_Hurted;
                    break;
                case 2:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted22KTX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted22TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX2K_Female_Average_Hurted2
                        : AndroidTiersTXSeriesHeads_DefOf.TX2K_Male_Average_Hurted2;
                    break;
                default:
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX2K_Female_Average_Normal
                        : AndroidTiersTXSeriesHeads_DefOf.TX2K_Male_Average_Normal;
                    break;
            }
        }

        if (pawn.def == AndroidTiersTXSeriesRaces_DefOf.ATPP_Android3TX)
        {
            switch (type)
            {
                case 1:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted13TX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted13TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX3_Female_Average_Hurted
                        : AndroidTiersTXSeriesHeads_DefOf.TX3_Male_Average_Hurted;
                    break;
                case 2:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted23TX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted23TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX3_Female_Average_Hurted2
                        : AndroidTiersTXSeriesHeads_DefOf.TX3_Male_Average_Hurted2;
                    break;
                default:
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX3_Female_Average_Normal
                        : AndroidTiersTXSeriesHeads_DefOf.TX3_Male_Average_Normal;
                    break;
            }
        }

        if (pawn.def == AndroidTiersTXSeriesRaces_DefOf.ATPP_Android4TX)
        {
            switch (type)
            {
                case 1:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted14TX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted14TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX4_Female_Average_Hurted
                        : AndroidTiersTXSeriesHeads_DefOf.TX4_Male_Average_Hurted;
                    break;
                case 2:
                    bodyToSet = female
                        ? AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeFemaleHurted24TX
                        : AndroidTiersTXSeriesBodies_DefOf.ATPP_BodyTypeMaleHurted24TX;
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX4_Female_Average_Hurted2
                        : AndroidTiersTXSeriesHeads_DefOf.TX4_Male_Average_Hurted2;
                    break;
                default:
                    headToSet = female
                        ? AndroidTiersTXSeriesHeads_DefOf.TX4_Female_Average_Normal
                        : AndroidTiersTXSeriesHeads_DefOf.TX4_Male_Average_Normal;
                    break;
            }
        }

        if (bodyToSet == pawn.story.bodyType && headToSet == pawn.story.headType)
        {
            return;
        }

        pawn.story.bodyType = bodyToSet;
        pawn.story.headType = headToSet;
        pawn.Drawer.renderer.graphics.ResolveAllGraphics();
        pawn.Drawer.renderer.graphics.ClearCache();
        PortraitsCache.SetDirty(pawn);
    }
}