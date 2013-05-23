using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.GameComponents;

namespace Carcassonne.Entities
{
    public class EntityTextures : IEquatable<MovingEntity>
    {
        #region Constructor

        public EntityTextures(Texture2D texture, Texture2D frame, Color color, Type entityType)
        {
            this.Texture = texture;
            this.Frame = frame;
            this.Color = color;
            this.EntityType = entityType;
        }

        #endregion

        #region Properties

        public Texture2D Texture
        {
            get;
            private set;
        }
        public Texture2D Frame
        {
            get;
            private set;
        }

        public Color Color
        {
            get;
            private set;
        }

        public Type EntityType
        {
            get;
            private set;
        }

        #endregion

        #region Implement IEquatable

        public bool Equals(MovingEntity typeToTest)
        {
            return (EntityType == (typeToTest.GetType()));
        }

        #endregion
    }
}
