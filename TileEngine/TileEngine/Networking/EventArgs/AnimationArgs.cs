namespace TileEngine.Networking.Args
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnimationArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public AnimationArgs(Vector2 location,int color,float scale)
        {
            this.Location = location;
            this.Color = color;
            this.Scale = scale;
        }

        #endregion

        #region Public Properties

        public Vector2 Location { get; private set; }

        public int Color { get; private set; }

        public float Scale { get; private set; }

        #endregion
    }
}