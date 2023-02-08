using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AndroidTiersTXSeries;

[StaticConstructorOnStartup]
public static class AndroidTiersTXSeries
{
    private static readonly List<HeadTypeDef> HeadsWithBlueEyes;
    private static readonly List<HeadTypeDef> HeadsWithRedEyes;
    private static readonly List<HeadTypeDef> AllGlowingHeads;
    public static TXBodyStuffGameComponent TXBodyStuff;

    private static readonly Dictionary<Pair<string, Color>, Graphic> EyeGlowEffectCache =
        new Dictionary<Pair<string, Color>, Graphic>();

    static AndroidTiersTXSeries()
    {
        new Harmony("Mlie.AndroidTiersTXSeries").PatchAll(Assembly.GetExecutingAssembly());
        HeadsWithBlueEyes = new List<HeadTypeDef>
        {
            AndroidTiersTXSeriesHeads_DefOf.TX3_Female_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX3_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX3_Female_Average_Hurted2,
            AndroidTiersTXSeriesHeads_DefOf.TX3_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX3I_Female_Average_Normal,
            AndroidTiersTXSeriesHeads_DefOf.TX3I_Male_Average_Normal,
            AndroidTiersTXSeriesHeads_DefOf.TX4_Female_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX4_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX4_Female_Average_Hurted2,
            AndroidTiersTXSeriesHeads_DefOf.TX4_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX4I_Female_Average_Normal,
            AndroidTiersTXSeriesHeads_DefOf.TX4I_Male_Average_Normal
        };

        HeadsWithRedEyes = new List<HeadTypeDef>
        {
            AndroidTiersTXSeriesHeads_DefOf.TX2K_Female_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX2K_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX2K_Female_Average_Hurted2,
            AndroidTiersTXSeriesHeads_DefOf.TX2K_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX2KI_Female_Average_Normal,
            AndroidTiersTXSeriesHeads_DefOf.TX2KI_Male_Average_Normal
        };

        var headsWithYellowEyes = new List<HeadTypeDef>
        {
            AndroidTiersTXSeriesHeads_DefOf.TX2_Female_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX2_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX2_Female_Average_Hurted2,
            AndroidTiersTXSeriesHeads_DefOf.TX2_Male_Average_Hurted,
            AndroidTiersTXSeriesHeads_DefOf.TX2I_Female_Average_Normal,
            AndroidTiersTXSeriesHeads_DefOf.TX2I_Male_Average_Normal
        };

        AllGlowingHeads = HeadsWithBlueEyes.Concat(HeadsWithRedEyes).Concat(headsWithYellowEyes).ToList();
    }

    private static Graphic getEyeGlowEffect(Color color, string gender, int type, int front)
    {
        var key = new Pair<string, Color>(type + gender + front, color);
        if (EyeGlowEffectCache.TryGetValue(key, out var value))
        {
            return value;
        }

        if (front == 1)
        {
            EyeGlowEffectCache[key] =
                GraphicDatabase.Get<Graphic_Single>(
                    (gender == "M" ? "Things/Misc/Androids/Effects/Front" : "Things/Misc/Androids/Effects/FFront") +
                    type, ShaderDatabase.MoteGlow, Vector2.one, color);
            return EyeGlowEffectCache[key];
        }

        EyeGlowEffectCache[key] =
            GraphicDatabase.Get<Graphic_Single>(
                (gender == "M" ? "Things/Misc/Androids/Effects/Side" : "Things/Misc/Androids/Effects/FSide") + type,
                ShaderDatabase.MoteGlow, Vector2.one, color);
        return EyeGlowEffectCache[key];
    }

    public static void DrawEyes(PawnRenderFlags renderFlags, Pawn pawn, float angle, Vector3 rootLoc, Rot4 bodyFacing,
        Rot4 headFacing)
    {
        if (pawn == null || !pawn.def.defName.StartsWith("ATPP_"))
        {
            return;
        }

        if (pawn.Dead || renderFlags.FlagSet(PawnRenderFlags.HeadStump))
        {
            return;
        }

        if (headFacing == Rot4.North)
        {
            return;
        }

        if (!AllGlowingHeads.Contains(pawn.story.headType))
        {
            return;
        }

        var portraitMode = renderFlags.FlagSet(PawnRenderFlags.Portrait);

        if (portraitMode && pawn.jobs?.curDriver?.asleep == true)
        {
            return;
        }

        var quaternion = Quaternion.AngleAxis(angle, Vector3.up);
        var type = 1;
        if (pawn.story.headType.defName.Contains("Hurted"))
        {
            type = 2;
        }

        var color = new Color(0.945f, 0.76862f, 0.05882f);
        if (HeadsWithBlueEyes.Contains(pawn.story.headType))
        {
            color = new Color(0f, 0.972549f, 0.972549f);
            type = 3;
        }

        if (HeadsWithRedEyes.Contains(pawn.story.headType) &&
            (pawn.Drafted || pawn.Faction?.HostileTo(Faction.OfPlayer) == true))
        {
            color = new Color(0.75f, 0f, 0f, 1f);
        }

        var a = rootLoc;
        if (bodyFacing != Rot4.North)
        {
            a.y += 0.0281250011f;
            rootLoc.y += 0.0234375f;
        }
        else
        {
            a.y += 0.0234375f;
            rootLoc.y += 0.0281250011f;
        }

        var b = quaternion * pawn.Drawer.renderer.BaseHeadOffsetAt(headFacing);
        var loc = a + b + new Vector3(0f, 0.01f, 0f);
        var mesh = MeshPool.humanlikeHeadSet.MeshAt(headFacing);
        var gender = "M";
        if (pawn.story.bodyType.defName.Contains("Female"))
        {
            gender = "F";
        }

        GenDraw.DrawMeshNowOrLater(mesh, loc, quaternion,
            headFacing.IsHorizontal
                ? getEyeGlowEffect(color, gender, type, 0).MatSingle
                : getEyeGlowEffect(color, gender, type, 1).MatSingle, true);
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
            if (type != 0)
            {
                return;
            }

            TXBodyStuff.OriginalHairs[pawn.ThingID] = pawn.story?.hairDef;
            TXBodyStuff.OriginalBeards[pawn.ThingID] = pawn.style?.beardDef;
            TXBodyStuff.OriginalBodyTattoos[pawn.ThingID] = pawn.style?.BodyTattoo;
            TXBodyStuff.OriginalFaceTattoos[pawn.ThingID] = pawn.style?.FaceTattoo;
            return;
        }


        pawn.story.bodyType = bodyToSet;
        pawn.story.headType = headToSet;

        switch (type)
        {
            case 0:
                if (TXBodyStuff.OriginalHairs.TryGetValue(pawn.ThingID, out var hair))
                {
                    pawn.story.hairDef = hair;
                }

                if (TXBodyStuff.OriginalBeards.TryGetValue(pawn.ThingID, out var beard))
                {
                    pawn.style.beardDef = beard;
                }

                if (TXBodyStuff.OriginalBodyTattoos.TryGetValue(pawn.ThingID, out var bodytatoo))
                {
                    pawn.style.BodyTattoo = bodytatoo;
                }

                if (TXBodyStuff.OriginalFaceTattoos.TryGetValue(pawn.ThingID, out var facetatoo))
                {
                    pawn.style.FaceTattoo = facetatoo;
                }

                break;
            default:
                pawn.story.hairDef = HairDefOf.Bald;
                pawn.style.beardDef = BeardDefOf.NoBeard;
                pawn.style.BodyTattoo = TattooDefOf.NoTattoo_Body;
                pawn.style.FaceTattoo = TattooDefOf.NoTattoo_Face;
                break;
        }

        pawn.Drawer.renderer.graphics.ClearCache();
        pawn.Drawer.renderer.graphics.CalculateHairMats();
        pawn.Drawer.renderer.graphics.ResolveAllGraphics();
        PortraitsCache.SetDirty(pawn);
        pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
    }
}