using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Modders
{
    public class ImageMod
    {
        private readonly Image image;

        public ImageMod(Image image)
        {
            this.image = image;
        }

        public Image Get()
        {
            return image;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}
