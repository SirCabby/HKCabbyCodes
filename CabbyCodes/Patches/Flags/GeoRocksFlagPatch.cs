using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;

namespace CabbyCodes.Patches.Flags
{
    public class GeoRocksFlagPatch : BasePatch
    {
        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.Crossroads_01__Geo_Rock_2,
                FlagInstances.Crossroads_05__Geo_Rock_1,
                FlagInstances.Crossroads_07__Geo_Rock_1,
                FlagInstances.Crossroads_07__Geo_Rock_1_1,
                FlagInstances.Crossroads_07__Geo_Rock_1_2,
                FlagInstances.Crossroads_08__Geo_Rock_1,
                FlagInstances.Crossroads_08__Geo_Rock_1_1,
                FlagInstances.Crossroads_08__Geo_Rock_1_2,
                FlagInstances.Crossroads_08__Geo_Rock_1_3,
                FlagInstances.Crossroads_12__Geo_Rock_2,
                FlagInstances.Crossroads_13__Geo_Rock_1,
                FlagInstances.Crossroads_13__Geo_Rock_2,
                FlagInstances.Crossroads_16__Geo_Rock_2,
                FlagInstances.Crossroads_18__Geo_Rock_1,
                FlagInstances.Crossroads_18__Geo_Rock_2,
                FlagInstances.Crossroads_18__Geo_Rock_3,
                FlagInstances.Crossroads_19__Geo_Rock_1,
                FlagInstances.Crossroads_36__Geo_Rock_1,
                FlagInstances.Crossroads_42__Geo_Rock_1,
                FlagInstances.Crossroads_42__Geo_Rock_2,
                FlagInstances.Crossroads_ShamanTemple__Geo_Rock_2_1,
                FlagInstances.Fungus1_01__Geo_Rock_Green_Path_01,
                FlagInstances.Fungus1_02__Geo_Rock_Green_Path_01_1,
                FlagInstances.Tutorial_01__Geo_Rock_1,
                FlagInstances.Tutorial_01__Geo_Rock_2,
                FlagInstances.Tutorial_01__Geo_Rock_3,
                FlagInstances.Tutorial_01__Geo_Rock_4,
                FlagInstances.Tutorial_01__Geo_Rock_5,
            };
        }
    }
} 