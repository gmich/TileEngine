using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public class MenuButton : GameButton
    {

        #region Constructor

        public MenuButton(Texture2D texture, SpriteFont font, Vector2 buttonLocation, string fontString, float alphaChangeRate)
            : base(texture, font, buttonLocation, fontString, alphaChangeRate)
        {
            ;
        }

        #endregion

         public override Vector2 ButtonLocation
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth / 2, location.Y);
            }
            set
            {
                base.ButtonLocation = value;
            }
        }

    }
}
