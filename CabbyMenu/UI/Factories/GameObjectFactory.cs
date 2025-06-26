using CabbyMenu.UI.Modders;
using UnityEngine;

namespace CabbyMenu.UI.Factories
{
    /// <summary>
    /// Abstract factory class for creating and configuring GameObject elements with their modifiers.
    /// </summary>
    public abstract class GameObjectFactory
    {
        /// <summary>
        /// Builds a GameObject with its associated GameObjectMod for easy manipulation.
        /// </summary>
        /// <param name="createdGameObject">The GameObject to wrap with a modifier.</param>
        /// <returns>A tuple containing the GameObject and its GameObjectMod.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod) Build(GameObject createdGameObject)
        {
            return (createdGameObject, new GameObjectMod(createdGameObject));
        }
    }
}