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
        private GameObject spriteDisplayPanel;
        private LayoutElement panelLayout;

        public SpriteDisplayPanel() : base("Sprite Display")
        {
            CreateSpriteDisplay();
        }

        private void CreateSpriteDisplay()
        {
            // Create a larger panel for sprite display
            var panelSize = new Vector2(200, 200);
            
            // Create sprite display panel using DefaultControls
            spriteDisplayPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            spriteDisplayPanel.name = "Sprite Display Panel";
            new ImageMod(spriteDisplayPanel.GetComponent<Image>()).SetColor(new Color(0.1f, 0.1f, 0.1f, 0.8f));
            new Fitter(spriteDisplayPanel).Attach(cheatPanel);
            spriteDisplayPanel.transform.SetAsFirstSibling();

            // Add LayoutElement to control size
            panelLayout = spriteDisplayPanel.AddComponent<LayoutElement>();
            panelLayout.preferredWidth = panelSize.x;
            panelLayout.preferredHeight = panelSize.y;
            panelLayout.minWidth = panelSize.x;
            panelLayout.minHeight = panelSize.y;

            // Create sprite display object
            spriteDisplayObject = new GameObject("SpriteDisplay");
            spriteDisplayObject.transform.SetParent(spriteDisplayPanel.transform, false);
            
            var spriteRect = spriteDisplayObject.AddComponent<RectTransform>();
            spriteRect.anchorMin = new Vector2(0.5f, 0.5f);
            spriteRect.anchorMax = new Vector2(0.5f, 0.5f);
            spriteRect.pivot = new Vector2(0.5f, 0.5f);
            spriteRect.anchoredPosition = Vector2.zero;

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

                    // --- Dynamic panel resizing logic ---
                    float nativeWidth = sprite.rect.width;
                    float nativeHeight = sprite.rect.height;
                    float scale = 2f;
                    float targetWidth = nativeWidth * scale;
                    float targetHeight = nativeHeight * scale;

                    // Resize the background panel and layout element
                    var panelRect = spriteDisplayPanel.GetComponent<RectTransform>();
                    panelRect.sizeDelta = new Vector2(targetWidth, targetHeight);
                    panelLayout.preferredWidth = targetWidth;
                    panelLayout.preferredHeight = targetHeight;
                    panelLayout.minWidth = targetWidth;
                    panelLayout.minHeight = targetHeight;

                    // Make the image fill the panel
                    var image = spriteImageMod.Get();
                    var rectTransform = image.rectTransform;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.sizeDelta = Vector2.zero;
                    image.preserveAspect = true;

                    // Force layout rebuild on this cheat panel and the parent cheat content area
                    LayoutRebuilder.ForceRebuildLayoutImmediate(cheatPanel.GetComponent<RectTransform>());
                    var parent = cheatPanel.transform.parent;
                    if (parent != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
                    }
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