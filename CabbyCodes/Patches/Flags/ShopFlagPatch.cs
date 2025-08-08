using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class ShopFlagPatch : BasePatch
    {
        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                // Sly Shop section
                new InfoPanel("Sly Shop").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateShopPanels(new[] { 
                FlagInstances.slyRancidEgg
            }));
            
            return panels;
        }
        
        private List<CheatPanel> CreateShopPanels(FlagDef[] flags)
        {
            var panels = new List<CheatPanel>();
            foreach (var flag in flags)
            {
                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            return panels;
        }
    }
} 