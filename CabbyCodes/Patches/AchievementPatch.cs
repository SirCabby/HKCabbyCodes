using CabbyCodes.SyncedReferences;
using CabbyCodes.Types;
using CabbyCodes.UI;
using CabbyCodes.UI.CheatPanels;
using CabbyCodes.UI.Modders;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.Patches
{
    public class AchievementPatch : ISyncedReference<bool>
    {
        private readonly Achievement achievement;
        private TogglePanel parent;

        public AchievementPatch(Achievement achievement)
        {
            this.achievement = achievement;
        }

        public bool Get()
        {
            return GameManager.instance.IsAchievementAwarded(achievement.key);
        }

        public void Set(bool value)
        {
            if (value && !Get())
            {
                AchievementHandler achievementHandler = Object.FindObjectOfType<AchievementHandler>();
                achievementHandler.AwardAchievementToPlayer(achievement.key);
                parent?.Update();
            }
        }

        public void SetCheatPanel(TogglePanel parent)
        {
            this.parent = parent;
        }

        private static TogglePanel BuildAchievementPanel(Achievement achievement)
        {
            string text = Language.Language.Get(achievement.localizedTitle, "Achievements") + ": " + Language.Language.Get(achievement.localizedText, "Achievements");
            AchievementPatch patch = new(achievement);

            TogglePanel togglePanel = new(patch, text);
            PanelAdder.AddSprite(togglePanel, achievement.earnedIcon, () => { return GameManager.instance.IsAchievementAwarded(achievement.key); }, 1);
            patch.SetCheatPanel(togglePanel);

            return togglePanel;
        }

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Toggle Achievements <ON> to unlock in-game and in Online platform (Steam / GOG)").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Unable to toggle off achievements independently").SetColor(CheatPanel.warningColor));
            ResetAchievementPatch.AddPanel();

            AchievementHandler achievementHandler = Object.FindObjectOfType<AchievementHandler>();
            foreach (Achievement achievement in achievementHandler.achievementsList.achievements)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(BuildAchievementPanel(achievement));
            }
        }
    }
}
