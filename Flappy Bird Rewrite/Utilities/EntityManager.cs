using System;
using System.Collections.Generic;
using Flappy_Bird_Rewrite.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flappy_Bird_Rewrite.Utilities
{
    public static class EntityManager
    {
        private static List<BaseEntity> _baseEntities = new List<BaseEntity>();
        private static List<BaseEntity> _awaitingEntities = new List<BaseEntity>();
        private static List<BaseEntity> _removedEntities = new List<BaseEntity>();
        private static bool _isUpdating;

        public static List<BaseEntity> Entities => _baseEntities;
        
        public static void UpdateEntities(GameTime gameTime)
        {
            _isUpdating = true;
            foreach (BaseEntity baseEntity in _baseEntities) // Update all the entities in the list.
            {
                baseEntity.Update(gameTime);
            }
            _isUpdating = false;

            foreach (BaseEntity baseEntity in _awaitingEntities) // If any are waiting to be added to the list, add them here.
            {
                _baseEntities.Add(baseEntity);
            }
            _awaitingEntities.Clear();
            
            foreach (BaseEntity baseEntity in _removedEntities) // If any are waiting to be removed from the list, remove them here.
            {
                _baseEntities.Remove(baseEntity);
            }
            _removedEntities.Clear();
        }

        public static void DrawEntities(SpriteBatch spriteBatch)
        {
            foreach (BaseEntity baseEntity in _baseEntities) // Loop through each entity and call it's draw function
            {
                baseEntity.Draw(spriteBatch);
            }
        }

        public static void AddEntity(BaseEntity baseEntity) // Adds an entity to either the _baseEntities list or the _awaitingEntities list depending on if the game is currently updating them.
        {
            if (_isUpdating) _awaitingEntities.Add(baseEntity);
            else _baseEntities.Add(baseEntity);
        }

        public static void DestroyEntity(BaseEntity baseEntity) // Similar to the AddEntity() method.
        {
            if (!_isUpdating) _baseEntities.Remove(baseEntity);
            else _removedEntities.Add(baseEntity);
        }

        public static void DestroyAllOfType(Type type)
        {
            foreach (BaseEntity baseEntity in _baseEntities)
            {
                Console.WriteLine(baseEntity.GetType());
                if (baseEntity.GetType() == type) DestroyEntity(baseEntity);
            }
        }

        public static bool IsEntityColliding(BaseEntity a, BaseEntity b) // Checks to see if the entities are intersecting. If they aren't don't bother with the heavy math.
        {
            return a != b &&
                   a.Position.X - a.CollisionBox.Width/2 <= b.Position.X - b.CollisionBox.Width/2 + b.CollisionBox.Width &&
                   a.Position.X - a.CollisionBox.Width/2 + a.CollisionBox.Width >= b.Position.X - b.CollisionBox.Width/2 &&
                   a.Position.Y - a.CollisionBox.Height/2 <= b.Position.Y - b.CollisionBox.Height/2 + b.CollisionBox.Height &&
                   a.Position.Y - a.CollisionBox.Height/2 + a.CollisionBox.Height >= b.Position.Y - b.CollisionBox.Height/2;
        }
    }
}