using System;
using Flappy_Bird_Rewrite.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Utilities
{
    public static class PipeManager
    {
        private const float PipeGap = 800;
        public static void GeneratePipe(float x)
        {
            float randomY = new Random().Next(-200, 150); // Generate a random X coordinate for the pipe.
            // Generate the two pipes
            Pipe pipe1 = new Pipe(new Vector2(x, randomY + PipeGap), FlappyBird.GetContentManager().Load<Texture2D>("Sprites/pipe"));
            Pipe pipe2 = new Pipe(new Vector2(x, randomY), FlappyBird.GetContentManager().Load<Texture2D>("Sprites/pipe"), true);

            Trigger trigger = // Generate the collision trigger to increment the score.
                new Trigger(
                    new Vector2(pipe2.Position.X + pipe2.Width / 2,
                        FlappyBird.GetGraphicsDeviceManager().PreferredBackBufferHeight / 2), 10, 1000);
            // Add the pipes & trigger
            EntityManager.AddEntity(pipe1);
            EntityManager.AddEntity(pipe2);
            EntityManager.AddEntity(trigger);
        }
    }
}