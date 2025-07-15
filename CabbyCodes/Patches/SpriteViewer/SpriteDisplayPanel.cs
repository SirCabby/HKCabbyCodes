using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;
using CabbyCodes.Patches.Hunter;
using CabbyMenu.UI;
using System.Linq;

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
                    // --- Dynamic panel resizing logic (do this first to set up the panel and image correctly) ---
                    float nativeWidth = sprite.rect.width;
                    float nativeHeight = sprite.rect.height;
                    float scale = 2f;
                    float targetWidth = nativeWidth * scale;
                    float targetHeight = nativeHeight * scale;

                    // We'll determine if we need to swap width/height after checking rotation below
                    bool flipX = false;
                    bool flipY = false;
                    bool rotate90 = false;
                    string flipModeStr = "None";

                    // Set the sprite first
                    spriteImageMod.SetSprite(sprite);

                    // Set up the image anchors and size to fill the panel
                    var image = spriteImageMod.Get();
                    var imageRectTransform = image.rectTransform;
                    imageRectTransform.anchorMin = Vector2.zero;
                    imageRectTransform.anchorMax = Vector2.one;
                    imageRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    imageRectTransform.anchoredPosition = Vector2.zero;
                    imageRectTransform.sizeDelta = Vector2.zero;
                    image.preserveAspect = true;

                    // --- Flip/rotation logic based on FlipMode and tk2dSprite FlipX/FlipY ---
                    try
                    {
                        // Find the tk2dSprite instance for this collection and sprite
                        var allSprites = Object.FindObjectsOfType<MonoBehaviour>()
                            .Where(mb => mb.GetType().Name == "tk2dSprite")
                            .ToArray();
                        foreach (var tk2dSprite in allSprites)
                        {
                            var collectionProp = tk2dSprite.GetType().GetProperty("Collection");
                            if (collectionProp == null) continue;
                            var collection = collectionProp.GetValue(tk2dSprite, null);
                            if (collection == null) continue;
                            var nameProp = collection.GetType().GetProperty("name");
                            string colName = nameProp != null ? (string)nameProp.GetValue(collection, null) : "<unknown>";
                            if (colName != collectionName) continue;

                            // Try to get spriteDefinitions
                            var spriteDefsProp = collection.GetType().GetProperty("spriteDefinitions");
                            System.Array spriteDefs = null;
                            if (spriteDefsProp != null)
                                spriteDefs = spriteDefsProp.GetValue(collection, null) as System.Array;
                            if (spriteDefs == null)
                            {
                                var spriteDefsField = collection.GetType().GetField("spriteDefinitions");
                                if (spriteDefsField != null)
                                    spriteDefs = spriteDefsField.GetValue(collection) as System.Array;
                            }
                            if (spriteDefs != null)
                            {
                                foreach (var def in spriteDefs)
                                {
                                    var nameField = def.GetType().GetField("name");
                                    string defName = nameField != null ? (string)nameField.GetValue(def) : null;
                                    if (defName == spriteName)
                                    {
                                        // --- Only log and use FlipMode flipped field ---
                                        var flipModeField = def.GetType().GetField("flipped");
                                        if (flipModeField != null)
                                        {
                                            var flipModeValue = flipModeField.GetValue(def);
                                            if (flipModeValue != null)
                                            {
                                                flipModeStr = flipModeValue.ToString();
                                                // Debug.Log($"[SpriteViewer] tk2dSpriteDefinition '{defName}' FlipMode flipped = {flipModeStr}");
                                                if (flipModeStr == "TPackerCW" || flipModeStr == "Tk2d")
                                                {
                                                    rotate90 = true;
                                                }
                                            }
                                        }

                                        // Only log and use tk2dSprite FlipX/FlipY fields/properties
                                        // var flipXProp = tk2dSprite.GetType().GetProperty("FlipX");
                                        // var flipYProp = tk2dSprite.GetType().GetProperty("FlipY");
                                        // if (flipXProp != null)
                                        // {
                                        //     flipX = (bool)flipXProp.GetValue(tk2dSprite, null);
                                        //     Debug.Log($"[SpriteViewer] tk2dSprite FlipX = {flipX}");
                                        // }
                                        // if (flipYProp != null)
                                        // {
                                        //     flipY = (bool)flipYProp.GetValue(tk2dSprite, null);
                                        //     Debug.Log($"[SpriteViewer] tk2dSprite FlipY = {flipY}");
                                        // }

                                        // Only process the first matching sprite
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                    }

                    // Now that we know if we need to rotate, swap width/height for the background panel if needed
                    if (rotate90)
                    {
                        (targetHeight, targetWidth) = (targetWidth, targetHeight);
                    }

                    // Resize the background panel and layout element
                    var panelRect = spriteDisplayPanel.GetComponent<RectTransform>();
                    panelRect.sizeDelta = new Vector2(targetWidth, targetHeight);
                    panelLayout.preferredWidth = targetWidth;
                    panelLayout.preferredHeight = targetHeight;
                    panelLayout.minWidth = targetWidth;
                    panelLayout.minHeight = targetHeight;

                    // Now apply rotation/flipping to the image
                    if (rotate90)
                    {
                        imageRectTransform.localEulerAngles = new Vector3(0, 0, 90f);
                    }
                    else
                    {
                        imageRectTransform.localEulerAngles = Vector3.zero;
                    }
                    imageRectTransform.localScale = new Vector3(flipX ? -1 : 1, flipY ? -1 : 1, 1);

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