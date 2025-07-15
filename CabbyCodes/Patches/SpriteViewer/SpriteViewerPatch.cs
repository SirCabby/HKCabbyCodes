using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.Patches.SpriteViewer
{
    public class SpriteViewerPatch
    {
        private static readonly Dictionary<string, object> spriteCollections = new Dictionary<string, object>();
        private static readonly Dictionary<string, List<string>> collectionSprites = new Dictionary<string, List<string>>();
        private static readonly BoxedReference<int> selectedCollectionIndex = new BoxedReference<int>(0);
        private static readonly BoxedReference<int> selectedSpriteIndex = new BoxedReference<int>(0);

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Sprite Viewer").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Browse and view sprites from currently loaded collections in this room").SetColor(CheatPanel.subHeaderColor));

            InitializeSpriteCollections();

            // Collection selector
            var collectionSelector = new CollectionSelector();
            var collectionDropdown = new DropdownPanel(collectionSelector, "Select Collection", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(collectionDropdown);

            // Sprite selector (will be recreated when collection changes)
            var spriteSelector = new SpriteSelector();
            var spriteDropdown = new DropdownPanel(spriteSelector, "Select Sprite", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(spriteDropdown);

            // Sprite display panel
            var spriteDisplay = new SpriteDisplayPanel();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(spriteDisplay);

            // Hook up the update chain
            collectionDropdown.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => 
            {
                spriteSelector.UpdateSpriteList();
                
                // Store the current panel's background colors
                var spriteDropdownColor = spriteDropdown.cheatPanel.GetComponent<Image>().color;
                var spriteDisplayColor = spriteDisplay.cheatPanel.GetComponent<Image>().color;
                
                // Remove both panels
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(spriteDropdown);
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(spriteDisplay);
                
                // Temporarily reverse the color pattern to maintain the same colors
                CheatPanel.ResetPattern();
                // Force the pattern to match the original colors
                if (spriteDropdownColor == CabbyMenu.Constants.PANEL_COLOR_2)
                {
                    // If the original was color2, we need to skip one iteration
                    var tempPanel = new InfoPanel("temp");
                    tempPanel.SetColor(CabbyMenu.Constants.PANEL_COLOR_1);
                }
                
                // Create a new sprite dropdown panel with the updated options
                spriteDropdown = new DropdownPanel(spriteSelector, "Select Sprite", Constants.DEFAULT_PANEL_HEIGHT);
                spriteDropdown.cheatPanel.GetComponent<Image>().color = spriteDropdownColor;
                
                // Create a new sprite display panel
                spriteDisplay = new SpriteDisplayPanel();
                spriteDisplay.cheatPanel.GetComponent<Image>().color = spriteDisplayColor;
                
                // Add both panels back to the menu in the correct order
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(spriteDropdown);
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(spriteDisplay);
                
                // Hook up the sprite selection event for the new dropdown
                spriteDropdown.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(value => 
                {
                    spriteDisplay.UpdateSprite();
                });
                
                // Update the sprite display
                spriteDisplay.UpdateSprite();
            });

            spriteDropdown.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => 
            {
                // Store the current panel's background color
                var spriteDisplayColor = spriteDisplay.cheatPanel.GetComponent<Image>().color;

                // Remove the current sprite display panel
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(spriteDisplay);

                // Create a new sprite display panel
                spriteDisplay = new SpriteDisplayPanel();
                spriteDisplay.cheatPanel.GetComponent<Image>().color = spriteDisplayColor;

                // Add the new panel back to the menu
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(spriteDisplay);
            });

            // Initialize with first collection if available
            if (spriteCollections.Count > 0)
            {
                selectedCollectionIndex.Set(0);
                spriteSelector.UpdateSpriteList();
                spriteDropdown.Update();
                spriteDisplay.UpdateSprite();
            }
        }

        private static void InitializeSpriteCollections()
        {
            spriteCollections.Clear();
            collectionSprites.Clear();

            try
            {
                // Find all tk2dSprite objects to get their collections
                var allSprites = Object.FindObjectsOfType<MonoBehaviour>()
                    .Where(mb => mb.GetType().Name == "tk2dSprite")
                    .ToArray();

                foreach (var sprite in allSprites)
                {
                    try
                    {
                        var collectionProp = sprite.GetType().GetProperty("Collection");
                        if (collectionProp == null) continue;

                        var collection = collectionProp.GetValue(sprite, null);
                        if (collection == null) continue;

                        // Get collection name
                        string collectionName = GetCollectionName(collection);
                        if (collectionName != "<unknown>" && !spriteCollections.ContainsKey(collectionName))
                        {
                            spriteCollections[collectionName] = collection;
                            
                            // Extract sprite names from this collection
                            var spriteNames = GetSpriteNamesFromCollection(collection);
                            collectionSprites[collectionName] = spriteNames;
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }

        public static string GetSelectedCollection()
        {
            var collectionNames = spriteCollections.Keys.OrderBy(k => k).ToList();
            var index = selectedCollectionIndex.Get();
            if (index >= 0 && index < collectionNames.Count)
            {
                return collectionNames[index];
            }
            return "";
        }

        public static string GetSelectedSprite()
        {
            var collectionName = GetSelectedCollection();
            if (string.IsNullOrEmpty(collectionName) || !collectionSprites.ContainsKey(collectionName))
            {
                return "";
            }
            
            var sprites = collectionSprites[collectionName];
            var index = selectedSpriteIndex.Get();
            if (index >= 0 && index < sprites.Count)
            {
                return sprites[index];
            }
            return "";
        }

        private static string GetCollectionName(object collection)
        {
            var nameProp = collection.GetType().GetProperty("name");
            if (nameProp != null)
            {
                var name = (string)nameProp.GetValue(collection, null);
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }

            return "<unknown>";
        }

        private static List<string> GetSpriteNamesFromCollection(object collection)
        {
            var spriteNames = new List<string>();

            try
            {
                var spriteDefsProp = collection.GetType().GetProperty("spriteDefinitions");
                System.Array spriteDefs = null;

                if (spriteDefsProp != null)
                {
                    spriteDefs = spriteDefsProp.GetValue(collection, null) as System.Array;
                }

                if (spriteDefs == null)
                {
                    var spriteDefsField = collection.GetType().GetField("spriteDefinitions");
                    if (spriteDefsField != null)
                    {
                        spriteDefs = spriteDefsField.GetValue(collection) as System.Array;
                    }
                }

                if (spriteDefs != null)
                {
                    for (int i = 0; i < spriteDefs.Length; i++)
                    {
                        var def = spriteDefs.GetValue(i);
                        if (def == null) continue;

                        string defName = GetSpriteDefinitionName(def);
                        if (!string.IsNullOrEmpty(defName))
                        {
                            spriteNames.Add(defName);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }

            return spriteNames.OrderBy(s => s).ToList();
        }

        private static string GetSpriteDefinitionName(object def)
        {
            var nameField = def.GetType().GetField("name");
            if (nameField != null)
            {
                var name = (string)nameField.GetValue(def);
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }

            return null;
        }

        private class CollectionSelector : ISyncedValueList
        {
            public int Get()
            {
                return selectedCollectionIndex.Get();
            }

            public void Set(int value)
            {
                selectedCollectionIndex.Set(value);
            }

            public List<string> GetValueList()
            {
                return spriteCollections.Keys.OrderBy(k => k).ToList();
            }
        }

        private class SpriteSelector : ISyncedValueList
        {
            public int Get()
            {
                return selectedSpriteIndex.Get();
            }

            public void Set(int value)
            {
                selectedSpriteIndex.Set(value);
            }

            public List<string> GetValueList()
            {
                var sprites = GetCurrentSpriteList();
                return sprites;
            }

            public void UpdateSpriteList()
            {
                var sprites = GetCurrentSpriteList();
                if (sprites.Count > 0)
                {
                    var currentSprite = GetSelectedSprite();
                    if (!sprites.Contains(currentSprite))
                    {
                        selectedSpriteIndex.Set(0);
                    }
                }
                else
                {
                    selectedSpriteIndex.Set(0);
                }
            }

            private List<string> GetCurrentSpriteList()
            {
                var collectionName = GetSelectedCollection();
                if (string.IsNullOrEmpty(collectionName))
                {
                    return new List<string>();
                }
                
                if (!collectionSprites.ContainsKey(collectionName))
                {
                    return new List<string>();
                }
                
                var sprites = collectionSprites[collectionName];
                return sprites;
            }
        }
    }
} 