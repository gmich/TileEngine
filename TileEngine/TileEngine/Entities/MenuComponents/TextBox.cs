using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Input;

namespace TileEngine.Entities.MenuComponents
{

    public class TextBox
    {

        #region Declarations

        private KeyboardInput keyboardManager;
        private Vector2 initialPos;
        private float timePassed;
        private bool MouseOver;
        private float Transparency;
        private bool lastActive;

        #endregion

        #region Constructor

        public TextBox(SpriteFont font,Texture2D texture,Vector2 location,int width,int height)
        {
            Font = font;
            timePassed = 0;
            Texture = texture;
            Location = location;
            Width = width;
            Height = height;
            InitialPosition = location;
            keyboardManager = new KeyboardInput();
            lastActive = Active = ShowPrompt = false;
            TextColor = Color.White;
            AlphaChangeRate=0.02f;
        }

        #endregion

        #region Properties

        #region Effects Properties

        private float AlphaChangeRate
        {
            get;
            set;
        }

        #endregion

        public Color TextColor
        {
            get;
            set;
        }
        private bool Active
        {
            get;
            set;
        }

        private bool ShowPrompt
        {
            get;
            set;
        }

        private SpriteFont Font
        {
            get;
            set;
        }

        private Vector2 PromptLocation
        {
            get
            {
                Vector2 stringSize=Font.MeasureString(keyboardManager.Text);
                return new Vector2(Location.X + stringSize.X, Location.Y );
            }
        }

        private Texture2D Texture
        {
            get;
            set;
        }

        private Vector2 InitialPosition
        {
            get
            {
                return new Vector2(initialPos.X + 5, initialPos.Y + Height / 2);
            }
            set
            {
                initialPos = value;
            }
        }

        public Vector2 Location
        {
            get;
            set;
        }

        private int Width
        {
            get;
            set;
        }

        private int Height
        {
            get;
            set;
        }

        private Rectangle LayoutRectangle
        {
            get { return new Rectangle((int)Location.X, (int)Location.Y, Width, Height); }
        }

        public string Text
        {
            get
            {
                return keyboardManager.Text;
            }
            set
            {
                keyboardManager.Text = value;
            }
        }

        #endregion

        #region Layout Activity

        public void LayoutActive()
        {
            if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                MouseOver = false;
                Active = false;
                return;
            }

            if (LayoutRectangle.Intersects(InputManager.MouseRectangle))
            {
                MouseOver=true;
                if(InputManager.LeftButtonIsClicked())
                Active = true;
            }
            else if (!LayoutRectangle.Intersects(InputManager.MouseRectangle))
            {
                MouseOver=false;
                if(InputManager.LeftButtonIsClicked())
                Active = false;
            }

        }

        #endregion

        #region Prompt Animation

        public void AnimatePrompt()
        {
            if (timePassed <300)
                ShowPrompt = true;
            else
                ShowPrompt = false;

            if (timePassed >= 600)
                timePassed = 0;
        }

        #endregion

        #region Effects

        public void UpdateTransparency()
        {
            if (Active)
            {
                Transparency = MathHelper.Min(1.0f, Transparency + AlphaChangeRate * 2);
                return;
            }

            if (MouseOver)
            {
                if (Transparency > 0.6)
                    Transparency = MathHelper.Max(0.7f, Transparency - AlphaChangeRate);
                else
                    Transparency = MathHelper.Min(0.7f, Transparency + AlphaChangeRate);
            }
            else
            {
                Transparency = MathHelper.Max(0.5f, Transparency - AlphaChangeRate);
            }
        }

        #endregion

        #region Update

        public bool IsTextBoxUpdated()
        {
            return (lastActive != Active && Active == false);
        }

        public void Update(GameTime gameTime)
        {
            timePassed += (float)gameTime.ElapsedGameTime.Milliseconds;

            lastActive = Active;

            AnimatePrompt();
            LayoutActive();
            UpdateTransparency();

            keyboardManager.BufferFull = (PromptLocation.X > Location.X + Width-20 );
         
            if (Active)
                keyboardManager.Update(gameTime);
            
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, LayoutRectangle, null,Color.White*Transparency,0.0f,Vector2.Zero,SpriteEffects.None,0.02f);
            spriteBatch.DrawString(Font, keyboardManager.Text, Location, TextColor);

            if(ShowPrompt && Active)
                spriteBatch.DrawString(Font, "_", PromptLocation, Color.White);
        }

        #endregion
    }
}
