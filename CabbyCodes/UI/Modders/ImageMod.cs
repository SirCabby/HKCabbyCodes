using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Modders
{
    public class ImageMod
    {
        private readonly Image image;
        public bool IsVisible
        {
            get { return image.enabled; }
            set { image.enabled = value; }
        }

        public ImageMod(Image image)
        {
            this.image = image;
        }

        public Image Get()
        {
            return image;
        }

        public ImageMod SetColor(Color color)
        {
            image.color = color;
            return this;
        }
    }
}
