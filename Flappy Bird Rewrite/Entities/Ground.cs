using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Entities
{
    public class Ground : Entity
    {
        public Ground(Vector2 position, Texture2D texture, float scale, bool flip) : base(position, texture, scale,
            flip ? 180 : 0)
        {
            RigidBodyPhysics = false;
            if (flip) SpriteEffects = SpriteEffects.FlipHorizontally;
        }
    }
}