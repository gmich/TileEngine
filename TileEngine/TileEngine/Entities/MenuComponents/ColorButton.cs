using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public class ColorButton : MenuButton
    {

        #region Constructor

        public ColorButton(Texture2D texture, SpriteFont font, Vector2 buttonLocation, string fontString, float alphaChangeRate)
            : base(texture, font, buttonLocation, fontString, alphaChangeRate)
        {
            FontColor = Color.Black;
        }

        #endregion

        public override Color ButtonColor
        {
            get
            {
                return FontColor;
            }
        }

    }
}
