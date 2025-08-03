using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CabbyCodes.Patches.SpriteViewer
{
    /// <summary>
    /// Manages sprites for use in SpriteViewer panels
    /// </summary>
    public class SpriteManager
    {
        private static SpriteManager instance;
        public static SpriteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpriteManager();
                }
                return instance;
            }
        }

        private readonly Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        private readonly Dictionary<string, object> spriteCollections = new Dictionary<string, object>();

        private SpriteManager()
        {
            InitializeSpriteCollections();
        }

        /// <summary>
        /// Initialize sprite collections by finding all tk2dSpriteCollectionData objects in the scene.
        /// </summary>
        private void InitializeSpriteCollections()
        {
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

                        string collectionName = GetCollectionName(collection);
                        if (collectionName != "<unknown>" && !spriteCollections.ContainsKey(collectionName))
                        {
                            spriteCollections[collectionName] = collection;
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

        /// <summary>
        /// Get the name of a sprite collection using reflection.
        /// </summary>
        private string GetCollectionName(object collection)
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

        /// <summary>
        /// Get a sprite by name from a specific collection.
        /// </summary>
        public Sprite GetSprite(string collectionName, string spriteName)
        {
            string cacheKey = $"{collectionName}:{spriteName}";
            
            if (spriteCache.ContainsKey(cacheKey))
            {
                return spriteCache[cacheKey];
            }

            try
            {
                if (!spriteCollections.ContainsKey(collectionName))
                {
                    return null;
                }

                var collection = spriteCollections[collectionName];
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

                // Find sprite by name
                for (int i = 0; i < spriteDefs.Length; i++)
                {
                    var def = spriteDefs.GetValue(i);
                    if (def == null) continue;

                    string defName = GetSpriteDefinitionName(def);
                    if (defName == spriteName)
                    {
                        var sprite = ConvertToUnitySprite(def);
                        if (sprite != null)
                        {
                            spriteCache[cacheKey] = sprite;
                            return sprite;
                        }
                    }
                }

                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the name of a sprite definition using reflection.
        /// </summary>
        private string GetSpriteDefinitionName(object def)
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

        /// <summary>
        /// Convert a tk2dSpriteDefinition to a Unity Sprite.
        /// This method handles the tk2dSpriteDefinition structure used in Hollow Knight.
        /// </summary>
        private Sprite ConvertToUnitySprite(object tk2dDef)
        {
            try
            {
                // Get the material from the tk2dSpriteDefinition
                var materialField = tk2dDef.GetType().GetField("material");
                if (materialField != null)
                {
                    var material = materialField.GetValue(tk2dDef) as Material;
                    if (material != null && material.mainTexture != null)
                    {
                        // Get the texture from the material
                        var texture = material.mainTexture as Texture2D;
                        if (texture != null)
                        {
                            // Get the UV coordinates from the tk2dSpriteDefinition
                            var uvsField = tk2dDef.GetType().GetField("uvs");
                            if (uvsField != null)
                            {
                                if (uvsField.GetValue(tk2dDef) is Vector2[] uvs && uvs.Length >= 4)
                                {
                                    // Calculate the sprite rect from UV coordinates
                                    float minU = Mathf.Min(uvs[0].x, uvs[1].x, uvs[2].x, uvs[3].x);
                                    float maxU = Mathf.Max(uvs[0].x, uvs[1].x, uvs[2].x, uvs[3].x);
                                    float minV = Mathf.Min(uvs[0].y, uvs[1].y, uvs[2].y, uvs[3].y);
                                    float maxV = Mathf.Max(uvs[0].y, uvs[1].y, uvs[2].y, uvs[3].y);

                                    float width = (maxU - minU) * texture.width;
                                    float height = (maxV - minV) * texture.height;
                                    float x = minU * texture.width;
                                    float y = minV * texture.height;

                                    return Sprite.Create(texture,
                                        new Rect(x, y, width, height),
                                        new Vector2(0.5f, 0.5f));
                                }
                            }
                            
                            // Fallback: use the entire texture
                            return Sprite.Create(texture, 
                                new Rect(0, 0, texture.width, texture.height), 
                                new Vector2(0.5f, 0.5f));
                        }
                    }
                }

                // Alternative: try to get the texture directly from the definition
                var textureField = tk2dDef.GetType().GetField("texture");
                if (textureField != null)
                {
                    var texture = textureField.GetValue(tk2dDef) as Texture2D;
                    if (texture != null)
                    {
                        return Sprite.Create(texture, 
                            new Rect(0, 0, texture.width, texture.height), 
                            new Vector2(0.5f, 0.5f));
                    }
                }

                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Clear the sprite cache to free memory.
        /// </summary>
        public void ClearCache()
        {
            spriteCache.Clear();
        }
    }
} 