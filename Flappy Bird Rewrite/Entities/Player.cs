using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flappy_Bird_Rewrite.Entities
{
    public class Player : Entity
    {
        private bool _isSpacePressed;
        private bool _isGameOver;

        public Player(Vector2 position, Texture2D texture, float scale) : base(position, texture, scale)
        {
            CollisionBox = new Rectangle((int) Position.X, (int) Position.Y, (int) Width - 20, (int) Height - 10); // Set the collision box for the player up.
        }

        public override void Update(GameTime gameTime)
        {
            Rotation = YVelocity * 5; // Make the bird point in the direction it is moving. The value of 5 should be increased later.
            if (!_isGameOver) // Runs this if the game is not over.
            {
                XVelocity = 1;
                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (!_isSpacePressed)
                    {
                        _isSpacePressed = true;
                        YVelocity = -6;
                    }
                }
                else _isSpacePressed = false;
            }
            else XVelocity = 0; // Set X velocity to 0, freezing the bird in place, only allowing gravity to work.
            base.Update(gameTime);
        }

        public override void OnCollision(BaseEntity baseEntity)
        {
            if (baseEntity.GetType() == typeof(Pipe)) _isGameOver = true; // Check to see if the bird is colliding with a pipe
            // TODO: Add ground collision
        }
    }
}