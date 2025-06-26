using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Modders
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

        public ImageMod SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
            return this;
        }
    }
}