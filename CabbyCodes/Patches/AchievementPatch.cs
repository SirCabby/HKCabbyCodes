using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using CabbyCodes.UI.Modders;
using UnityEngine;

namespace CabbyCodes.Patches
{
    public class AchievementPatch : ISyncedReference<bool>
    {
        private static readonly Color unearnedColor = new(0.57f, 0.57f, 0.57f, 0.57f);

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
            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, achievement.earnedIcon, 1);
            patch.SetCheatPanel(togglePanel);

            togglePanel.updateActions.Add(() =>
            {
                if (GameManager.instance.IsAchievementAwarded(achievement.key))
                {
                    spriteImageMod.SetColor(Color.white);
                }
                else
                {
                    spriteImageMod.SetColor(unearnedColor);
                }
            });
            togglePanel.Update();

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
