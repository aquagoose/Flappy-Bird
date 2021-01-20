using Flappy_Bird_Rewrite.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Entities
{
    public class Trigger : Entity
    {
        private bool _hasIncreasedScore;
        
        public Trigger(Vector2 position, int width, int height) : base(position, new Texture2D(FlappyBird.GetGraphicsDevice(), width, height))
        {
            // Set up the trigger flags. Triggers are blank entities with only a collision box. Used for.. Well, triggering stuff, heh
            CollisionBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            RigidBodyPhysics = false;
            UsesCollision = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (FlappyBird.Camera.Position.X - Position.X - Width/2 >= 0) EntityManager.DestroyEntity(this); // Again, destroys the trigger if it's off screen. Should be improved.
            base.Update(gameTime);
        }

        public override void OnCollision(BaseEntity baseEntity)
        {
            if (baseEntity.GetType() == typeof(Player) && !_hasIncreasedScore) // Increase the score of the player. This only runs once.
            {
                FlappyBird.Score++;
                _hasIncreasedScore = true;
            }
        }
    }
}