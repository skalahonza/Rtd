using UnityEngine;

namespace Assets.Mechanics
{
    public static class ImageMechanics
    {
        /// <summary>
        /// Load image sprite
        /// </summary>
        /// <param name="name">Name of the image</param>
        /// <returns>Sprite from file</returns>
        public static Sprite LoadSprite(string name)
        {
            const string dir = "Sprites";
            var sprite = Resources.Load<Sprite>(dir+"/" + name);
            return sprite;
        }
    }
}