using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public interface IButton
    {
        #region Button Dimensions

        Vector2 ButtonLocation
        {
            get;
            set;
        }

        Texture2D Texture
        {
            get;
            set;
        }

        int OffSet
        {
            get;
            set;
        }

        void setButtonDimensions(float width, float height);

        #endregion
        
        #region Font Methods

        void SetFontColor(Color color);

        void SetFont(string fontString);

        #endregion

        #region Mouse Events

        bool IsClicked();

        #endregion

        #region Update

        void Update(GameTime gameTime);
        
        #endregion

        #region Draw

        void Draw(SpriteBatch spriteBatch);

        #endregion
    }
}
