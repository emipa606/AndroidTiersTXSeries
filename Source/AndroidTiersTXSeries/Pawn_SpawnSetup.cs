using HarmonyLib;
using Verse;

namespace AndroidTiersTXSeries;

[HarmonyPatch(typeof(Pawn), "SpawnSetup")]
public static class Pawn_SpawnSetup
{
    public static void Postfix(ref Pawn __instance)
    {
        AndroidTiersTXSeries.UpdatePawnGraphics(ref __instance);
    }
}