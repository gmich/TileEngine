using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Diagnostics
{
    public class FpsMonitor
    {
        public float Value { get; private set; }
        public TimeSpan Sample { get; set; }
        private Vector2 Location { get; set; }
        private SpriteFont Font { get; set; }
        private Stopwatch sw;
        private int Frames;

        public FpsMonitor(SpriteFont Font, Vector2 Location)
        {
            this.Font = Font;
            this.Location = Location;
            this.Sample = TimeSpan.FromSeconds(1);
            this.Value = 0;
            this.Frames = 0;
            this.sw = Stopwatch.StartNew();
        }

        public void Update(GameTime gameTime)
        {
            if (sw.Elapsed > Sample)
            {
                this.Value = (float)(Frames / sw.Elapsed.TotalSeconds);
                this.sw.Reset();
                this.sw.Start();
                this.Frames = 0;
            }
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            this.Frames++;
            SpriteBatch.DrawString(Font, "FPS: " + this.Value.ToString(), Location, Color.Black);
        }
    }
}