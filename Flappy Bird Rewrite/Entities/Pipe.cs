using System;
using Flappy_Bird_Rewrite.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Entities
{
    public class Pipe : Entity
    {

        public Pipe(Vector2 position, Texture2D texture, bool flip = false) : base(position, texture, 1, flip ? 180 : 0)
        {
            RigidBodyPhysics = false;
            if (flip) SpriteEffects = SpriteEffects.FlipHorizontally;
            CollisionBox = new Rectangle((int) Position.X, (int) Position.Y, (int) Width - 25, (int) Height - 30); // Set the collision box for the pipe
        }

        public override void Update(GameTime gameTime)
        {
            if (FlappyBird.Camera.Position.X - Position.X - Width/2 >= 0) EntityManager.DestroyEntity(this); // Destroy the pipe if it is outside the screen
        }
    }
}