using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Modders
{
    public class GameObjectMod
    {
        private readonly GameObject gameObject;

        public GameObjectMod(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public GameObject Get()
        {
            return gameObject;
        }

        public GameObjectMod SetName(string name)
        {
            gameObject.name = name;
            return this;
        }

        public GameObjectMod SetOutline(Color color)
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            return this;
        }
    }
}
