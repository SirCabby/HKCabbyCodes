using CabbyCodes.UI.Modders;
using UnityEngine;

namespace CabbyCodes.UI.Factories
{
    public abstract class GameObjectFactory
    {
        public static (GameObject gameObject, GameObjectMod gameObjectMod) Build(GameObject createdGameObject)
        {
            return (createdGameObject, new GameObjectMod(createdGameObject));
        }
    }
}
