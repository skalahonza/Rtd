using UnityEngine;

namespace Assets.Mechanics
{
    public static class ImageMechanics
    {
        public static Sprite LoadSprite(string name)
        {
            const string dir = "Sprites";
            var sprite = Resources.Load<Sprite>(dir+"/" + name);
            return sprite;
        }
    }
}