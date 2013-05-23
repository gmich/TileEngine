using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities
{
    public class MapSquare
    {
        public MapSquare(Texture2D texture, bool occupied)
        {
            this.Texture = texture;
            this.Occupied = occupied;
        }

        #region Properties

        public Texture2D Texture
        {
            get;
            set;
        }

        public bool Occupied
        {
            get;
            set;
        }

        #endregion
    }
}
