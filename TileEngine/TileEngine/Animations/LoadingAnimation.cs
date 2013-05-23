using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;
using TileEngine.Entities.MenuComponents;

namespace TileEngine.Animations
{
    public class LoadingAnimation : IAnimation
    {

        #region Declarations

        SpriteFont font;
        bool flipped;
        CenteredTitle title;
        Vector2 location;

        #endregion

        #region Constructor

        public LoadingAnimation(SpriteFont font)
        {
            this.font = font;
            title = new CenteredTitle(font, Vector2.Zero, " ", false, Color.Black);
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
            get
            {
                if (Location.X > Camera.ViewPortWidth)
                    flipped = true;

                else if (Location.X < 0)
                    flipped = false;

                if (flipped)
                    return new Vector2(-1, 0);
                else
                    return new Vector2(1, 0);
            }
        }

        private float Step
        {
            get
            {
                if (Location.X < Camera.ViewPortWidth / 2)
                    return Vector2.Distance(Location, new Vector2((float)Camera.ViewPortWidth, Location.Y));
                else
                    return Vector2.Distance(Location, new Vector2(0, Location.Y));
            }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            Location += Velocity * Step * gameTime.ElapsedGameTime.Milliseconds / 1000;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            title.Draw(spriteBatch);
            spriteBatch.DrawString(font, "...", Location, Color.Black);
        }
    }
}
