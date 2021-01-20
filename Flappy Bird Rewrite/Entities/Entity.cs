using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Entities
{
    /// <summary>
    /// Create a basic entity. By default these do nothing.
    /// </summary>
    public class Entity : BaseEntity
    {
        public Entity(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
            Scale = new Vector2(1f);
            Rotation = 0f;
        }

        public Entity(Vector2 position, Texture2D texture, float scale, float rotation = 0f)
        {
            Position = position;
            Texture = texture;
            Scale = new Vector2(scale);
            Rotation = rotation;
        }

        public Entity(Vector2 position, Texture2D texture, Vector2 scale, float rotation = 0f)
        {
            Position = position;
            Texture = texture;
            Scale = scale;
            Rotation = rotation;
        }
    }
}