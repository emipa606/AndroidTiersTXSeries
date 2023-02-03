using RimWorld;
using Verse;

namespace AndroidTiersTXSeries;

[DefOf]
public static class AndroidTiersTXSeriesRaces_DefOf
{
    public static ThingDef ATPP_Android2TX;
    public static ThingDef ATPP_Android2KTX;
    public static ThingDef ATPP_Android3TX;
    public static ThingDef ATPP_Android4TX;

    static AndroidTiersTXSeriesRaces_DefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(AndroidTiersTXSeriesRaces_DefOf));
    }
}