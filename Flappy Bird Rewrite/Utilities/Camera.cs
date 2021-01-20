using System;
using Microsoft.Xna.Framework;

namespace Flappy_Bird_Rewrite.Utilities
{
    public class Camera
    {
        private GraphicsDeviceManager _graphics;
        public Vector2 Position;
        public float Zoom = 1f;
        public float Rotation = 0f;

        public Vector2 Origin =>
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) / 2;
        
        public Camera(Vector2 position, GraphicsDeviceManager graphics) // Create the camera, with the graphics. Without the graphics, game no work, graphic good, we need graphic
        {
            Position = position;
            _graphics = graphics;
        }

        public Matrix GetViewMatrix(float z = 0f)
        {
            return Matrix.CreateTranslation(new Vector3(-Position, z)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, z)) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateTranslation(new Vector3(Origin, z)); // Create the transformation (view) matrix. Needs improving.
        }
    }
}