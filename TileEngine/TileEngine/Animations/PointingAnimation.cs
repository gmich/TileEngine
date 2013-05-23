using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;

namespace TileEngine.Animations
{
    public class PointingAnimation : IAnimation
    {
        Texture2D circleTexture; 
        Vector2 location;
        Vector2 initialLocation;
        float initialScale;
        float timePassed;
        int counter;

        public PointingAnimation(Texture2D circleTexture,Vector2 finalLocation,Color color)
        {
            this.circleTexture = circleTexture;
            this.InitialLocation = finalLocation;
            this.location = finalLocation -= new Vector2(90, 90);
            this.Color = color;
            timePassed = 0;
            this.initialScale = Camera.Scale;
            counter = 0;
            Alive = true;
        }

        #region Properties

        private Color Color
        {
            get;
            set;
        }

        public bool Alive
        {
            get;
            private set;
        }

        public string Text
        {
            set
            {
                string text = value;
            }
        }

        public Vector2 InitialLocation
        {
            get
            {
                return Camera.WorldToScreen(initialLocation * (Camera.Scale / initialScale));
            }
            set
            {
                initialLocation = value;
            }
        }

        public Vector2 Location
        {
            get
            {
                if (location == initialLocation || counter >= 50)
                    Alive = false;
                return Camera.WorldToScreen(location * (Camera.Scale / initialScale));
            }
            set
            {
                location = value;
            }
        }

        private Rectangle PointRectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, (int)(InitialLocation.X - Location.X) * 2, (int)(InitialLocation.X - Location.X) * 2);
            }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            timePassed= MathHelper.Min(timePassed + gameTime.ElapsedGameTime.Milliseconds,10);
            if (timePassed >= 3)
            {
                timePassed = 0;
                counter++;
                location += new Vector2(1, 1) * (Vector2.Distance(InitialLocation,Location)/15);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(circleTexture, PointRectangle, TileGrid.GetSolidColor(Color));
        }
    }
}
