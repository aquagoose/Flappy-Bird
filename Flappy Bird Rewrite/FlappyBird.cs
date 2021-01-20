using System;
using Flappy_Bird_Rewrite.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Flappy_Bird_Rewrite.Utilities;
using Microsoft.Xna.Framework.Content;

namespace Flappy_Bird_Rewrite
{
    public class FlappyBird : Game
    {
        public static int Score = 0;
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Camera Camera;
        private static ContentManager _contentManager;
        private static GraphicsDeviceManager _graphicsDeviceManager; // NOT the main graphics device, please note.
        private SpriteFont _arial;

        private Texture2D _playerTexture;
        private Player _player;

        private bool _canSpawn;

        public FlappyBird()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnWindowResize;
        }

        protected override void Initialize()
        {

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            //_graphics.PreferredBackBufferWidth = 1920;
            //_graphics.PreferredBackBufferHeight = 1080;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here

            Camera = new Camera(new Vector2(0, 0), _graphics);
            _contentManager = Content;
            _graphicsDeviceManager = _graphics;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _arial = Content.Load<SpriteFont>("Fonts/Arial-24px");
            AssetManager.AddTexture("Sprites/flappybird"); // This does not work, thus, is not currently used.

            _playerTexture = Content.Load<Texture2D>("Sprites/flappybird");
            //_player = new Player(new Vector2(100, 100), _playerTexture, 0.25f);
            _player = new Player(new Vector2(100, 100), _playerTexture, 0.25f);
            EntityManager.AddEntity(_player);

            PipeManager.GeneratePipe(1280);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            EntityManager.UpdateEntities(gameTime); // Update all the entities

            Camera.Position.X = _player.Position.X - 100; // Set the camera's position to be the players position - 100 (making it appear on the left)
            
            if ((int) _player.Position.X % 500 <= 10) // Spawn a pipe if the player's X coord is within this range.
            {
                if (_canSpawn) // This prevents multiple pipes from spawning, as the value is under 10 for a few frames.
                {
                    _canSpawn = false;
                    PipeManager.GeneratePipe(1280 + _player.Position.X);
                }
            }
            else _canSpawn = true;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());
            
            EntityManager.DrawEntities(_spriteBatch);
            _spriteBatch.DrawString(_arial, Debug.DebugText, new Vector2(0, 0), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            _spriteBatch.End();
            
            _spriteBatch.Begin(); // Second spritebatch to draw text & UI elements.

            _spriteBatch.DrawString(_arial, Score.ToString(), new Vector2(0, 0), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public static ContentManager GetContentManager()
        {
            return _contentManager;
        }

        public static GraphicsDevice GetGraphicsDevice()
        {
            return _graphicsDeviceManager.GraphicsDevice;
        }
        
        public static GraphicsDeviceManager GetGraphicsDeviceManager()
        {
            return _graphicsDeviceManager;
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            //_graphics.ApplyChanges();
        }
    }
}