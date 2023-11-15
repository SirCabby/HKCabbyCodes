using CabbyCodes.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.Patches;

namespace CabbyCodes.UI.CheatPanels
{
    public class AchievementPanel : TogglePanel
    {
        private static readonly int defaultWidth = 60;
        private static readonly Vector2 middle = new(0.5f, 0.5f);
        private static readonly Color unearnedColor = new(0.57f, 0.57f, 0.57f, 0.57f);

        private readonly ImageMod iconImageMod;

        public AchievementPanel(Achievement achievement)
            : base(null, Language.Language.Get(achievement.localizedTitle, "Achievements") + ": " + Language.Language.Get(achievement.localizedText, "Achievements"))
        {
            GameObject imagePanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            imagePanel.name = "Achievement Icon Panel";
            new ImageMod(imagePanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(imagePanel).Attach(cheatPanel);
            imagePanel.transform.SetSiblingIndex(1);

            LayoutElement imagePanelLayout = imagePanel.AddComponent<LayoutElement>();
            imagePanelLayout.flexibleHeight = 1;
            imagePanelLayout.minWidth = defaultWidth;

            GameObject achievementIcon = DefaultControls.CreateImage(new DefaultControls.Resources());
            Color achievementColor = Color.white;
            if (!GameManager.instance.IsAchievementAwarded(achievement.key))
            {
                achievementColor = unearnedColor;
            }
            iconImageMod = new ImageMod(achievementIcon.GetComponent<Image>()).SetSprite(achievement.earnedIcon).SetColor(achievementColor);
            new Fitter(achievementIcon).Attach(imagePanel).Anchor(middle, middle).Size(new Vector2(defaultWidth, defaultWidth));

            GetToggleButton().SetIsOn(new AchievementPatch(achievement, this));
        }

        public void AwardIcon()
        {
            iconImageMod.SetColor(Color.white);
        }
    }
}
