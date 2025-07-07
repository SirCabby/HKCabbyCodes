using System;
using UnityEngine;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Save game data structure (extends vanilla game structure with custom scene/position data).
    /// </summary>
    [Serializable]
    public class SaveGameData
    {
        public PlayerData playerData;
        public SceneData sceneData;
        public string sceneName;
        public float playerX;
        public float playerY;

        public SaveGameData(PlayerData playerData, SceneData sceneData, string sceneName, Vector2 playerPosition)
        {
            this.playerData = playerData;
            this.sceneData = sceneData;
            this.sceneName = sceneName;
            playerX = playerPosition.x;
            playerY = playerPosition.y;
        }

        public Vector2 GetPlayerPosition()
        {
            return new Vector2(playerX, playerY);
        }
    }
} 