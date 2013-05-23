using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.GameComponents;
using TileEngine;

namespace Carcassonne.Entities
{
    public class EntityInfo : IEquatable<MovingEntity>
    {
        Rectangle boundaries;
        Vector2 location;
        int aliveEntities;

        public EntityInfo(Texture2D entityFrame, SpriteFont font,MovingEntity entity, Color color, int maxEntities, Vector2 spawnLocation, Rectangle destructionBoundaries)
        {
            this.EntityFrame = entityFrame;
            this.Font = font;
            this.Entity = entity;
            this.Color = color;
            this.MaxEntities =  maxEntities;
            this.aliveEntities = maxEntities;
            this.SpawnLocation = spawnLocation;
            this.boundaries = destructionBoundaries;
        }

        #region Properties

        private SpriteFont Font
        {
            get;
            set;
        }

        private Texture2D EntityFrame
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        MovingEntity Entity
        {
            get;
            set;
        }

        public Type EntityType
        {
            get
            {
                return Entity.GetType();
            }
        }

        public int MaxEntities
        {
            get;
            private set;
        }

        public int AliveEntities
        {
            get
            {
                return aliveEntities;
            }
            set
            {
                aliveEntities = (int)MathHelper.Clamp(value, 0, MaxEntities);
            }
        }

        public Vector2 SpawnLocation
        {
            get
            {
                OffSetX = Camera.ViewPortWidth - location.X;
                return location;
            }
            private set
            {
                location = value;
            }
        }

        private Vector2 TextLocation
        {
            get
            {
                if (EntityType == typeof(LargeSoldier))
                    return location + new Vector2(5, 0);
                else
                    return location;
            }
        }

        private float OffSetX
        {
            get;
            set;
        }

        public Rectangle DestructionBoundaries
        {
            get
            {
                return new Rectangle(boundaries.X + (int)Camera.Position.X, boundaries.Y + (int)Camera.Position.Y, boundaries.Width, boundaries.Height);
            }
        }

        #endregion

        public void AdjustLocation()
        {
            location.X =Camera.ViewPortWidth - (int)OffSetX;
            boundaries.X = Camera.ViewPortWidth - boundaries.Width;
        }

        #region Implement IEquatable

        public bool Equals(MovingEntity typeToTest)
        {
            return (EntityType == (typeToTest.GetType())
                    && Color == typeToTest.Color);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(EntityFrame,boundaries, Color.White);
            spriteBatch.DrawString(Font, AliveEntities.ToString(), TextLocation, Color.Black);
        }

        #endregion
    }
}
