using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public class Title
    {
        protected Vector2 textLocation,textureLocation;
        protected Vector2 fontSize;

        public Title(SpriteFont font, Vector2 location, string text, bool showTexture, Color fontColor)
        {
            //Using viewport width coordinates
            this.TextureLocation = this.TextLocation = new Vector2(Camera.ViewPortWidth, 0) - location;
            this.Font = font;
            this.Text = text;
            this.ShowTexture = showTexture;
            this.FontColor = fontColor;
            fontSize = font.MeasureString(text);
        }

        #region Properties

        public Texture2D Texture
        {
            get;
            set;
        }

        private SpriteFont Font
        {
            get;
            set;
        }

        protected Vector2 FontSize
        {
            get
            {
                return Font.MeasureString(Text); 
            }
        }

        public virtual Vector2 TextLocation
        {
            get
            {
                if (ShowTexture)
                    return new Vector2(TextureLocation.X + Texture.Width - 4, TextureLocation.Y);
                else
                    return new Vector2(Camera.ViewPortWidth, 0) - textLocation;
            }
            set
            {
                textLocation = value;
            }
        }

        public virtual Vector2 TextureLocation
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth, 0) - textureLocation;
            }
            set
            {
                textureLocation = value;
            }
        }

        public string Text
        {
            get;
            set;
        }

        public Color FontColor
        {
            get;
            set;
        }

        public bool ShowTexture
        {
            get;
            set;
        }

        Vector2 Origin
        {
            get
            {
                return new Vector2(Texture.Width / 2, Texture.Height / 2);
            }
        }

        #endregion

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (ShowTexture)
                spriteBatch.Draw(Texture, TextureLocation, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.02f);

            spriteBatch.DrawString(Font,Text, TextLocation, FontColor);
        }
    }
}
