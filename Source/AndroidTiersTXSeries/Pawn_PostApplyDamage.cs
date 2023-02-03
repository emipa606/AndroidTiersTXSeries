using HarmonyLib;
using Verse;

namespace AndroidTiersTXSeries;

[HarmonyPatch(typeof(Pawn), "PostApplyDamage")]
public static class Pawn_PostApplyDamage
{
    public static void Postfix(ref Pawn __instance)
    {
        AndroidTiersTXSeries.UpdatePawnGraphics(ref __instance);
    }
}