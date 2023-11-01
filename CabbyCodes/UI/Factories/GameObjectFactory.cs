using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Factories
{
    public class GameObjectFactory<T> where T : GameObjectFactory<T>
    {
        protected GameObject buildInstance;

        public T SetName(string name)
        {
            buildInstance.name = name;
            return (T)this;
        }

        public T SetOutline(Color color)
        {
            Outline outline = buildInstance.AddComponent<Outline>();
            outline.effectColor = color;
            return (T)this;
        }

        public GameObject Build()
        {
            GameObject result = buildInstance;
            buildInstance = null;
            return result;
        }
    }
}
