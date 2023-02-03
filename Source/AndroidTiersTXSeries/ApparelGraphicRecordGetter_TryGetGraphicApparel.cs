using HarmonyLib;
using RimWorld;

namespace AndroidTiersTXSeries;

[HarmonyPatch(typeof(ApparelGraphicRecordGetter), "TryGetGraphicApparel")]
public static class ApparelGraphicRecordGetter_TryGetGraphicApparel
{
    public static void Prefix(ref BodyTypeDef bodyType)
    {
        if (!bodyType.defName.StartsWith("ATPP_"))
        {
            return;
        }

        if (bodyType.defName.Contains("Female"))
        {
            bodyType = BodyTypeDefOf.Female;
            return;
        }

        bodyType = BodyTypeDefOf.Male;
    }
}