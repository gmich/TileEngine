using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public class ScoreButton : GameButton
    {

        #region Constructor

        public ScoreButton(Texture2D texture, SpriteFont font, Vector2 buttonLocation, string fontString, float alphaChangeRate)
            :base(texture,font,buttonLocation,fontString,alphaChangeRate)
        {
            //TODO: update info
        }

        #endregion

        public override int MaxOffSet
        {
            get
            {
                return 0;
            }
        }

        public override float MinAlphaValue
        {
            get
            {
                return 0.4f;
            }
        }

        public override Vector2 ButtonLocation
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth - location.X, location.Y);
            }
            set
            {
                location = value;
            }
        }

    }
}
