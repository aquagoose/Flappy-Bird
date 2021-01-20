using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Utilities
{
    public static class AssetManager
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

        public static Texture2D GetTexture(string textureName)
        {
            try
            {
                return Textures[textureName];
            }
            catch (KeyNotFoundException)
            {
                Debug.Write($"A texture tried to be accessed but could not be found. (Key: {textureName})");
                return null;
            }
        }

        public static void AddTexture(string textureName)
        {
            try
            {
                Textures.Add(textureName, FlappyBird.GetContentManager().Load<Texture2D>(textureName));
            }
            catch (ContentLoadException)
            {
                Debug.Write($"An asset tried to be loaded but could not be found. (Key: {textureName})");
            }
        }
    }
}