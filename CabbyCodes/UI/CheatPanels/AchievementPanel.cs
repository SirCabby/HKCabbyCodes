using CabbyCodes.Patches;
using CabbyCodes.UI.Modders;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace CabbyCodes.UI.CheatPanels
{
    public class AchievementPanel : TogglePanel
    {
        public AchievementPanel(Achievement achievement, BoxedReference IsOn, BasePatch togglePatch)
            : base(IsOn, togglePatch, Language.Language.Get(achievement.localizedTitle, "Achievements") + ": " + Language.Language.Get(achievement.localizedText, "Achievements"))
        {
            Fit(achievement);
        }

        public AchievementPanel(Achievement achievement, BoxedReference IsOn, Action toggleAction)
            : base(IsOn, toggleAction, Language.Language.Get(achievement.localizedTitle, "Achievements") + ": " + Language.Language.Get(achievement.localizedText, "Achievements"))
        {
            Fit(achievement);
        }

        private void Fit(Achievement achievement)
        {
            int width = 120;

            GameObject imagePanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            imagePanel.name = "Achievement Icon Panel";
            new ImageMod(imagePanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(imagePanel).Attach(cheatPanel);
            imagePanel.transform.SetSiblingIndex(1);

            LayoutElement imagePanelLayout = imagePanel.AddComponent<LayoutElement>();
            imagePanelLayout.flexibleHeight = 1;
            imagePanelLayout.minWidth = width/2;

            GameObject achievementIcon = DefaultControls.CreateImage(new DefaultControls.Resources());
            Color achievementColor = Color.white;
            if (!GameManager.instance.IsAchievementAwarded(achievement.key))
            {
                achievementColor = new Color(0.57f, 0.57f, 0.57f, 0.57f);
            }
            new ImageMod(achievementIcon.GetComponent<Image>()).SetSprite(achievement.earnedIcon).SetColor(achievementColor);
            new Fitter(achievementIcon).Attach(imagePanel).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).Size(new Vector2(width/2, 60));
        }
    }
}
