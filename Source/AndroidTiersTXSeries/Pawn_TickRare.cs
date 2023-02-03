using HarmonyLib;
using Verse;

namespace AndroidTiersTXSeries;

[HarmonyPatch(typeof(Pawn), "TickRare")]
public static class Pawn_TickRare
{
    public static void Prefix(ref Pawn __instance)
    {
        AndroidTiersTXSeries.UpdatePawnGraphics(ref __instance);
    }
}