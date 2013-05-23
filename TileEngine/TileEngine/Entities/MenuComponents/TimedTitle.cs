using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Entities.MenuComponents
{
    public class TimedTitle:CenteredTitle
    {

        #region Constructor

        public TimedTitle(SpriteFont font, Vector2 location, string text, bool showTexture, Color fontColor)
            :base(font,location,text,showTexture,fontColor)
        {
            TimeSinceText = TextTime = 501.0f;
        }

        #endregion

        #region Properties

        float TimeSinceText
        {
            get;
            set;
        }

        float TextTime
        {
            get;
            set;
        }

        bool ShowText
        {
            get
            {
                return (TimeSinceText < TextTime);
            }
        }

        public bool TimePassed
        {
            get
            {
                return (TimeSinceText > 500f);
            }
        }
        #endregion

        #region Public Methods

        public void SetText(string text,float time)
        {
            this.Text = text;
            TimeSinceText = 0.0f;
            TextTime = time;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            TimeSinceText = MathHelper.Min(gameTime.ElapsedGameTime.Milliseconds + TimeSinceText, TextTime + 1.0f);
        }

        #endregion

        #region Override Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ShowText)
                base.Draw(spriteBatch);
        }

        #endregion
    }
}
