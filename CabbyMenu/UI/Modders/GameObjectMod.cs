using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Modders
{
    /// <summary>
    /// Modifier class for manipulating GameObject properties with method chaining support.
    /// </summary>
    public class GameObjectMod
    {
        /// <summary>
        /// The GameObject being modified.
        /// </summary>
        private readonly GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the GameObjectMod class.
        /// </summary>
        /// <param name="gameObject">The GameObject to modify.</param>
        public GameObjectMod(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Gets the underlying GameObject.
        /// </summary>
        /// <returns>The GameObject being modified.</returns>
        public GameObject Get()
        {
            return gameObject;
        }

        /// <summary>
        /// Sets the name of the GameObject.
        /// </summary>
        /// <param name="name">The new name for the GameObject.</param>
        /// <returns>This GameObjectMod instance for method chaining.</returns>
        public GameObjectMod SetName(string name)
        {
            gameObject.name = name;
            return this;
        }

        /// <summary>
        /// Sets the active state of the GameObject.
        /// </summary>
        /// <param name="active">Whether the GameObject should be active.</param>
        /// <returns>This GameObjectMod instance for method chaining.</returns>
        public GameObjectMod SetActive(bool active)
        {
            gameObject.SetActive(active);
            return this;
        }

        /// <summary>
        /// Gets the current active state of the GameObject.
        /// </summary>
        /// <returns>True if the GameObject is active, false otherwise.</returns>
        public bool IsActive()
        {
            return gameObject.activeSelf;
        }

        /// <summary>
        /// Adds an outline component to the GameObject with the specified color.
        /// </summary>
        /// <param name="color">The color of the outline effect.</param>
        /// <returns>This GameObjectMod instance for method chaining.</returns>
        public GameObjectMod SetOutline(Color color)
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            return this;
        }
    }
}