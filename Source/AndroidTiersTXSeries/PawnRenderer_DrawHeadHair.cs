using HarmonyLib;
using UnityEngine;
using Verse;

namespace AndroidTiersTXSeries;

[HarmonyPatch(typeof(PawnRenderer), "DrawHeadHair")]
public static class PawnRenderer_DrawHeadHair
{
    public static void Postfix(Vector3 rootLoc, float angle, Rot4 bodyFacing, Rot4 headFacing, PawnRenderFlags flags,
        Pawn ___pawn)
    {
        AndroidTiersTXSeries.DrawEyes(flags, ___pawn, angle, rootLoc, bodyFacing, headFacing);
    }
}