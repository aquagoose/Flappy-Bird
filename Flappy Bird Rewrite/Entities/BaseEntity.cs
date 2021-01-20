using System;
using System.Linq;
using Flappy_Bird_Rewrite.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Entities
{
    /// <summary>
    /// The base entity class. Used for all objects that get drawn to the screen.
    /// </summary>
    public abstract class BaseEntity
    {
        private const float Gravity = 9.81f;
        
        #region Public Properties
        /// <summary>
        /// The entity's current position (Type: Vec2)
        /// </summary>
        public Vector2 Position; // The entity position
        /// <summary>
        /// The entity's current texture (Type: Tex2D)
        /// </summary>
        public Texture2D Texture; // The entity texture
        /// <summary>
        /// The entity's scale (Type: Vec2)
        /// </summary>
        public Vector2 Scale; // Entity scale
        /// <summary>
        /// If false, the entity will not respond to collision, instead acting like a trigger. (Type: bool)
        /// </summary>
        public bool UsesCollision = true;
        /// <summary>
        /// How strongly the entity will react to gravity, measured in G. (1G = 9.81m/s^2)
        /// Set this to 0 to disable gravity. Accepts negative values. (Type: float)
        /// </summary>
        public float GravityLevel = 1f;
        /// <summary>
        /// If set to false, none of the gravity & velocity calculations will occur. The object will essentially be immovable.
        /// Useful for static objects like trees etc. (Type: bool)
        /// </summary>
        public bool RigidBodyPhysics = true;
        /// <summary>
        /// The X and Y velocity. Used for movement & physics.
        /// </summary>
        public float XVelocity, YVelocity;

        /// <summary>
        /// The entity's max physics speed in any direction.
        /// </summary>
        //public float MaxSpeed = 20.0f;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        #endregion

        #region Private Fields
        private float _rotation; // Entity rotation
        private Rectangle _collisionBox; // The collision box. Used for... cake making, idk.
        private float _layerDepth; // The depth at which the object is drawn. Clamped within 0-1.
        #endregion

        #region Getter/Setter Properties
        /// <summary>
        /// Get & set the current rotation of the entity in degrees. (Type: float)
        /// </summary>
        public float Rotation
        {
            get => MathHelper.ToDegrees(_rotation); // Rotation is stored in degrees so must be converted to radians.
            set => _rotation = MathHelper.ToRadians(value);
        }

        /// <summary>
        /// Get & set the current rotation of the entity in radians. (Type: float)
        /// </summary>
        public float RotationInRadians
        {
            get => _rotation;
            set => _rotation = value;
        }

        /// <summary>
        /// Get the width of the entity. Note: NOT equivalent to getting the texture width. (Type: float)
        /// </summary>
        public float Width => Texture.Width * Scale.X;
        
        /// <summary>
        /// Get the height of the entity. Note: NOT equivalent to getting the texture height. (Type: float)
        /// </summary>
        public float Height => Texture.Height * Scale.Y;

        /// <summary>
        /// The collision box for this entity. Collision calculations will only occur from within this box. (Type: Vec2)
        /// </summary>
        public Rectangle CollisionBox
        {
            get => _collisionBox == default ? new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height) : _collisionBox;
            set => _collisionBox = value;
        }

        /// <summary>
        /// The depth at which the entity should be drawn. Clamped between 0 and 1. (Type: float)
        /// </summary>
        public float LayerDepth
        {
            get => _layerDepth;
            set => _layerDepth = Math.Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Get the top left corner of the collision box.
        /// </summary>
        public Vector2 TopLeft => new Vector2(Position.X - Width/2, Position.Y - Height/2);
        #endregion
        
        #region Overridable Methods

        /// <summary>
        /// Called once per update frame. It's best to keep 'base.Update()' inside the overriden method, as this
        /// allows the object to perform it's collision & physics calculations.
        /// </summary>
        /// <param name="gameTime">The current Game Time.</param>
        public virtual void Update(GameTime gameTime)
        {
            CalculatePhysics(gameTime); // Perform the physics calculations
            CalculateCollision(gameTime); // Perform the collision calculations.
            
            if (Position.Y > 10000) EntityManager.DestroyEntity(this); // This removes anything beyond Y 10000, to save memory & drawing cycles.
        }

        /// <summary>
        /// Called once per draw frame.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch that should be drawn to.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, _rotation, new Vector2(Texture.Width, Texture.Height) / 2, Scale,
                SpriteEffects, LayerDepth);

            /*Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, CollisionBox.Width, CollisionBox.Height);
            Color[] data = new Color[CollisionBox.Width * CollisionBox.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            rect.SetData(data);
            spriteBatch.Draw(rect, new Vector2(Position.X - CollisionBox.Width/2, Position.Y - CollisionBox.Height/2), Color.Black * 0.5f);*/
        }

        public virtual void OnCollision(BaseEntity baseEntity) { }

        #endregion

        #region Private Methods
        private void CalculatePhysics(GameTime gameTime)
        {
            if (!RigidBodyPhysics) return; // If RigidBody is disable, don't apply physics.
            YVelocity += Gravity * GravityLevel * (float)gameTime.ElapsedGameTime.TotalSeconds; // Apply gravity.
            // Apply X and Y positional movement based on velocity.
            Position.X += XVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds * 100;
            Position.Y += YVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            // TODO: Finish physics calculations
        }

        private void CalculateCollision(GameTime gameTime)
        {
            foreach (BaseEntity baseEntity in EntityManager.Entities.Where(baseEntity =>
                EntityManager.IsEntityColliding(this, baseEntity) && baseEntity != this)) // Check if there is an intersection. If there is not, do not run the code.
            {
                OnCollision(baseEntity); // Tell the entity that it is colliding with said entity.
                if (!baseEntity.UsesCollision) continue; // If Collision is disabled (trigger), just continue, effectively ignoring the code below.

                // ------- Calculate the rectangle intersection, to determine which side has been intersected ------------
                Rectangle intersect = Rectangle.Intersect(
                    new Rectangle((int) Position.X - CollisionBox.Width / 2, (int) Position.Y - CollisionBox.Height / 2,
                        CollisionBox.Width, CollisionBox.Height),
                    new Rectangle((int) baseEntity.Position.X - baseEntity.CollisionBox.Width / 2,
                        (int) baseEntity.Position.Y - baseEntity.CollisionBox.Height / 2, baseEntity.CollisionBox.Width,
                        baseEntity.CollisionBox.Height));
                // -------------------------------------------------------------------------------------------------------

                if (intersect.Height > intersect.Width) // If the height is greater than the width, it means you're on the right or left side of the box.
                {
                    Position.X -= XVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds * 100; // Inverse the velocity.
                    XVelocity = 0;
                }
                else
                {
                    Position.Y -= YVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds * 100;
                    YVelocity = 0;
                }
            }
        }
        #endregion
    }
}