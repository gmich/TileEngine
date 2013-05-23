using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Banner
{
    public interface IBanner
    {

        #region Properties

        string PlayerName
        {
            get;
            set;
        }

        string PlayerAvatarName
        {
            get;
            set;
        }

        Color Color
        {
            get;
            set;
        }

        Vector2 Location
        {
            get;
            set;
        }

        bool ShowAvatar
        {
            get;
            set;
        }

        bool AwaitAvatar
        {
            get;
        }

        bool Signal
        {
            get;
        }

        int Score
        {
            get;
            set;
        }

        #endregion

        Texture2D GetAvatar();

        void LoadGameAvatar(GraphicsDevice graphicsDevice);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
