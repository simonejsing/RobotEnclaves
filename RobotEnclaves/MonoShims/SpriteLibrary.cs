using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoShims
{
    public class SpriteLibrary
    {
        public enum SpriteIdentifier
        {
            Player,
            Block,
            Spikes,
            Coin
        }

        private readonly ContentManager Manager;
        private readonly Texture2D[] textures;

        public SpriteLibrary(ContentManager manager)
        {
            Manager = manager;
            textures = new Texture2D[sizeof(SpriteIdentifier)];
        }

        public void LoadSprite(SpriteIdentifier identifier, string name)
        {
            textures[(int)identifier] = Manager.Load<Texture2D>(name);
        }

        public Texture2D GetSprite(SpriteIdentifier identifier)
        {
            return textures[(int)identifier];
        }
    }
}
