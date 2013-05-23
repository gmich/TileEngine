using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    using TileEngine.Input;

    public class GameButton : IButton
    {

        protected Vector2 location;
        private Texture2D texture;

        #region Constructor

        public GameButton(Texture2D texture, SpriteFont font, Vector2 buttonLocation, string fontString, float alphaChangeRate)
        {
            Texture = texture;
            Font = font;

            Width = Texture.Width;
            Height = Texture.Height;
            ButtonLocation = new Vector2(buttonLocation.X + Width / 2, buttonLocation.Y + Height / 2);
            SetFont(fontString);

            AlphaChangeRate = alphaChangeRate;
            Transparency = 0.1f;
            Scale = 1.0f;
            Layer = 0.05f;
            IdleMouseOver = IsOnHold = IsLocked = false;
            FontColor = Color.Black;
            OffSet = 0;
        }

        #endregion

        #region Properties

        #region Texture/Font

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
        
        private SpriteFont Font
        {
            get;
            set;
        }

        private String FontString
        {
            get;
            set;
        }

        private Vector2 FontSize
        {
            get;
            set;
        }

        public Color FontColor
        {
            get;
            set;
        }

        public virtual float MinAlphaValue
        {
            get
            {
                return 0.8f;
            }
        }

        public virtual Color ButtonColor
        {
            get
            {
                if (IsOnHold)
                    return new Color(235, 235, 235);
                else
                    return Color.White;
            }
        }

        private bool UsingFont
        {
            get;
            set;
        }

        #endregion

        #region Location / Size / Rectangle

        public virtual Vector2 ButtonLocation
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public int OffSet
        {
            get;
            set;
        }

        public virtual int MaxOffSet
        {
            get
            {
                if (!IsOnHold)
                    return Texture.Height / 15;
                else
                    return Texture.Height / 15 + 2;
            }
        }

        private float TimePassed
        {
            get;
            set;
        }

        private Vector2 FontLocation
        {
            get
            {
                return new Vector2(ButtonLocation.X + (OffSet/2) - (FontSize.X / 2), ButtonLocation.Y + (OffSet/3) - (FontSize.Y / 2));
            }
        }

        private float TextScale
        {
            get
            {
                return 1.0f - OffSet * 0.011f;
            }
        }


        private Rectangle ButtonRectangle
        {
            get
            {
                return new Rectangle((int)ButtonLocation.X - (int)Width / 2, (int)ButtonLocation.Y - (int)Height / 2, (int)Width, (int)Height);
            }
        }

        private Rectangle ButtonScreenRectangle
        {
            get
            {
                return new Rectangle((int)ButtonLocation.X - (int)Width / 2 + OffSet, (int)ButtonLocation.Y - (int)Height / 2 + OffSet, (int)Width - (OffSet*2), (int)Height - (OffSet*2));
            }
        }

        private Vector2 TextureOrigin
        {
            get
            {
                return new Vector2(Width/2,Height/2);
            }
        }

        #endregion

        #region Mouse Response

        private bool IsLocked
        {
            get;
            set;
        }

        private bool IsOnHold
        {
            get;
            set;
        }

        private bool IdleMouseOver
        {
            get;
            set;
        }

        #endregion

        #region Effects

        private float AlphaChangeRate
        {
            get;
            set;
        }

        private float Transparency
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public float Layer
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Button Dimensions

        public void setButtonDimensions(float width, float height)
        {
            Width = width;
            Height = height;
        }

        #endregion

        #region Font Methods
        public void SetFont(string fontString)
        {
            if (fontString != null)
            {
                FontString = fontString;
                FontSize = Font.MeasureString(fontString);
                UsingFont = true;
            }
            else
                UsingFont = false;
        }

        public void SetFontColor(Color color)
        {
            FontColor = color;
        }

        #endregion

        #region Mouse Events

        public bool IsClicked()
        {
 
            if (InputManager.LeftButtonIsClicked())
            {
                if (!ButtonRectangle.Intersects(InputManager.MouseRectangle))
                {
                    IsLocked=true;
                }
                else if (ButtonRectangle.Intersects(InputManager.MouseRectangle) && !IsOnHold && !IsLocked)
                {
                    IsOnHold = true;
                    return false;
                }
  
            }
            else if (!InputManager.LeftButtonIsClicked())
            {
                IsLocked = false;
                if (!ButtonRectangle.Intersects(InputManager.MouseRectangle))
                {
                    IsOnHold = false;
                }
                else if (IsOnHold && ButtonRectangle.Intersects(InputManager.MouseRectangle))
                {
                    IsOnHold = false;
                    return true;
                }     
            }
            return false;
        }

        #endregion

        #region Effects Methods

        public virtual void UpdateTransparency(bool onHold)
        {
            if (IdleMouseOver)
            {
                Transparency = 1.0f;
            }
            else
            {
                Transparency = MathHelper.Max(MinAlphaValue, Transparency - AlphaChangeRate);
            }
        }

        private void UpdateOffSet(float elapsedTime)
        {
            int offSet = 0;
            TimePassed = MathHelper.Min(TimePassed + elapsedTime, 10.0f);
            if (TimePassed == 10.0f)
            {
                offSet = 1;
                TimePassed = 0.0f;
            }

            if (IdleMouseOver)
            {
                    OffSet = (int)MathHelper.Min(OffSet + offSet,MaxOffSet);
            }
            else
            {
                OffSet = (int)MathHelper.Max(OffSet - offSet, 0);
            }
        }


        #endregion

        #region Update

        public virtual void Update(GameTime gameTime)
        {
            if (!ButtonRectangle.Intersects(InputManager.MouseRectangle))
            {
                IdleMouseOver = false;
            }
            else
                IdleMouseOver = true;

            UpdateTransparency(IsOnHold);

            if (IdleMouseOver || (OffSet != 0))
                UpdateOffSet(gameTime.ElapsedGameTime.Milliseconds);

        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,ButtonScreenRectangle,null,ButtonColor*Transparency,0.0f,Vector2.Zero,SpriteEffects.None,Layer);

            if (UsingFont)
                spriteBatch.DrawString(Font, FontString, FontLocation, FontColor, 0.0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}
