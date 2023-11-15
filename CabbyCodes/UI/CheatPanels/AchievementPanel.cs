using CabbyCodes.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.Patches;

namespace CabbyCodes.UI.CheatPanels
{
    public class AchievementPanel : TogglePanel
    {
        private readonly ImageMod iconImageMod;

        public AchievementPanel(Achievement achievement)
            : base(null, Language.Language.Get(achievement.localizedTitle, "Achievements") + ": " + Language.Language.Get(achievement.localizedText, "Achievements"))
        {
            int width = 60;

            GameObject imagePanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            imagePanel.name = "Achievement Icon Panel";
            new ImageMod(imagePanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(imagePanel).Attach(cheatPanel);
            imagePanel.transform.SetSiblingIndex(1);

            LayoutElement imagePanelLayout = imagePanel.AddComponent<LayoutElement>();
            imagePanelLayout.flexibleHeight = 1;
            imagePanelLayout.minWidth = width;

            GameObject achievementIcon = DefaultControls.CreateImage(new DefaultControls.Resources());
            Color achievementColor = Color.white;
            if (!GameManager.instance.IsAchievementAwarded(achievement.key))
            {
                achievementColor = new Color(0.57f, 0.57f, 0.57f, 0.57f);
            }
            iconImageMod = new ImageMod(achievementIcon.GetComponent<Image>()).SetSprite(achievement.earnedIcon).SetColor(achievementColor);
            new Fitter(achievementIcon).Attach(imagePanel).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).Size(new Vector2(width, width));

            GetToggleButton().SetIsOn(new AchievementPatch(achievement, this));
        }

        public void AwardIcon()
        {
            iconImageMod.SetColor(Color.white);
        }
    }
}
