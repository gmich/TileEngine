using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Animations
{
    public interface IAnimation
    {
        #region Properties

        bool Alive
        {
            get;
        }

        string Text
        {
            set;
        }

        Vector2 InitialLocation
        {
            set;
        }

        Vector2 Location
        {
            get;
            set;
        }

        #endregion

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
