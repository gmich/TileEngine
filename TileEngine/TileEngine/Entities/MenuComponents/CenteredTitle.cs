using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public class CenteredTitle:Title
    {

        public CenteredTitle(SpriteFont font, Vector2 location, string text, bool showTexture, Color fontColor)
            :base(font,location,text,showTexture,fontColor)
        {
            ;
        }

        public override Vector2 TextureLocation
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth / 2 - Texture.Width / 2, textureLocation.Y);
            }
        }

        public override Vector2 TextLocation
        {
            get
            {
                if (ShowTexture)
                    return new Vector2(textureLocation.X + Texture.Width - 4, textureLocation.Y);
                else
                    return new Vector2(Camera.ViewPortWidth / 2 - FontSize.X / 2, -textureLocation.Y);
            }

        }
     
    }
}
