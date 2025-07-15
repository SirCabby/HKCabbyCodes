using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;
using CabbyCodes.Patches.Hunter;
using CabbyMenu.UI;

namespace CabbyCodes.Patches.SpriteViewer
{
    public class SpriteDisplayPanel : CheatPanel
    {
        private GameObject spriteDisplayObject;
        private ImageMod spriteImageMod;

        public SpriteDisplayPanel() : base("Sprite Display")
        {
            CreateSpriteDisplay();
        }

        private void CreateSpriteDisplay()
        {
            // Create a larger panel for sprite display
            var panelSize = new Vector2(200, 200);
            
            // Create sprite display panel using DefaultControls
            GameObject spriteDisplayPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            spriteDisplayPanel.name = "Sprite Display Panel";
            new ImageMod(spriteDisplayPanel.GetComponent<Image>()).SetColor(new Color(0.1f, 0.1f, 0.1f, 0.8f));
            new Fitter(spriteDisplayPanel).Attach(cheatPanel);
            spriteDisplayPanel.transform.SetAsFirstSibling();

            // Add LayoutElement to control size
            LayoutElement panelLayout = spriteDisplayPanel.AddComponent<LayoutElement>();
            panelLayout.preferredWidth = panelSize.x;
            panelLayout.preferredHeight = panelSize.y;
            panelLayout.minWidth = panelSize.x;
            panelLayout.minHeight = panelSize.y;

            // Create sprite display object
            spriteDisplayObject = new GameObject("SpriteDisplay");
            spriteDisplayObject.transform.SetParent(spriteDisplayPanel.transform, false);
            
            var spriteRect = spriteDisplayObject.AddComponent<RectTransform>();
            spriteRect.anchorMin = Vector2.zero;
            spriteRect.anchorMax = Vector2.one;
            spriteRect.offsetMin = new Vector2(10, 10);
            spriteRect.offsetMax = new Vector2(-10, -10);

            var spriteImage = spriteDisplayObject.AddComponent<Image>();
            spriteImageMod = new ImageMod(spriteImage);
            spriteImageMod.SetColor(Color.white);

            // Set initial text
            UpdateSprite();
        }

        public void UpdateSprite()
        {
            var collectionName = SpriteViewerPatch.GetSelectedCollection();
            var spriteName = SpriteViewerPatch.GetSelectedSprite();
            
            if (string.IsNullOrEmpty(collectionName) || string.IsNullOrEmpty(spriteName))
            {
                spriteImageMod.SetSprite(null);
                return;
            }

            try
            {
                // Get sprite from EnemySpriteManager (which handles the conversion)
                var sprite = EnemySpriteManager.Instance.GetSprite(collectionName, spriteName);
                if (sprite != null)
                {
                    spriteImageMod.SetSprite(sprite);
                }
                else
                {
                    spriteImageMod.SetSprite(null);
                }
            }
            catch (System.Exception)
            {
                spriteImageMod.SetSprite(null);
            }
        }
    }
} 