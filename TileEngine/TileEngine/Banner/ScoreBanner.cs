using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Entities.MenuComponents;

namespace TileEngine.Banner
{
    public class ScoreBanner
    {
        public int score;
        private Vector2 location;

        #region Constructor

        public ScoreBanner(Texture2D texture, SpriteFont font, SpriteFont largeFont, Vector2 location, Color color)
        {
            buttons = new List<IButton>();
            
            this.color = color;
            this.Font = font;
            this.LargeFont = largeFont;
            Score = 0;

            Vector2 OffSet = new Vector2(texture.Width, 0);
            Vector2 HorizontalOffSet = new Vector2(0, texture.Height);
            this.Location = location + HorizontalOffSet*2+ new Vector2(-45,-2);

            buttons.Add(new ScoreButton(texture, font, location - 2 * OffSet, "-10", 0.04f));
            buttons.Add(new ScoreButton(texture, font, location - 2 * OffSet + HorizontalOffSet, "+10", 0.04f));
            buttons.Add(new ScoreButton(texture, font, location + HorizontalOffSet-  OffSet, "+1", 0.04f));
            buttons.Add(new ScoreButton(texture, font, location -  OffSet , "-1", 0.04f));

            foreach (IButton button in buttons)
                button.SetFontColor(color);
        }

        #endregion

        #region Properties

        public bool Updated
        {
            get;
            set;
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                //fire event
                score = (int) MathHelper.Clamp( value, 0, 9999);
            }
        }

        Vector2 Location
        {
            get
            {
                return new Vector2(Camera.ViewPortWidth - location.X - (FontSize.X) + 13, location.Y);
            }
            set
            {
                location = value;
            }
        }

        Vector2 FontSize
        {
            get
            {
                return LargeFont.MeasureString(Score.ToString());
            }
        }

        Color color
        {
            get;
            set;
        }

        SpriteFont Font
        {
            get;
            set;
        }

        SpriteFont LargeFont
        {
            get;
            set;
        }

        #endregion

        #region Declarations

        List<IButton> buttons;

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            Updated = false;

            if (buttons[0].IsClicked())
            {
                Updated = true;
                Score -= 10;
            }
            else if (buttons[1].IsClicked())
            {
                Updated = true;
                Score += 10;
            }
            else if (buttons[2].IsClicked())
            {
                Updated = true;
                Score += 1;
            }
            else if (buttons[3].IsClicked())
            {
                Updated = true;
                Score -= 1;
            }

            foreach (IButton button in buttons)
                button.Update(gameTime);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IButton button in buttons)
                button.Draw(spriteBatch);

            spriteBatch.DrawString(LargeFont, Score.ToString(), Location, new Color(51,51,51));
        }

        #endregion
    }
}
