using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;
using TileEngine.Entities.MenuComponents;

namespace TileEngine.Animations
{
    public class CircularAnimation : IAnimation
    {

        #region Declarations

        SpriteFont font;
        CenteredTitle title;
        Vector2 location;
        float timePassed;

        #endregion

        #region Constructor

        public CircularAnimation(SpriteFont font,float radius)
        {
            this.font = font;
            title = new CenteredTitle(font, Vector2.Zero, " ", false, Color.Black);
            Velocity = new Vector2(1, 1);
            this.Radius = radius;
        }

        #endregion

        #region Properties

        public bool Alive
        {
            get
            {
                return true;
            }
        }

        public string Text
        {
            set
            {
                title.Text = value;
            }
        }

        public Vector2 InitialLocation
        {
            set
            {
                title.TextureLocation = value;
            }
        }

        public Vector2 Location
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

        private Vector2 Velocity
        {
            get;
            set;
        }

        private float Step
        {
            get
            {
                return 200f;
            }
        }

        private float Radius
        {
            get;
            set;
        }

        private Vector2 PolarToCartesianConversion(float theta)
        {
            return new Vector2((float)Math.Cos(theta),(float)Math.Sin(theta));  
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            timePassed += gameTime.ElapsedGameTime.Milliseconds * 0.008f;
            Velocity = PolarToCartesianConversion(timePassed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);
            spriteBatch.DrawString(font, "...", Location+(Velocity*Radius), Color.Black);
        }
    }
}
