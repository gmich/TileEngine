namespace TileEngine.Networking.Args
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestEntityArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public RequestEntityArgs(int tileID, int color,int counter)
        {
            this.TileID = tileID;
            this.Color = color;
            this.Counter = counter;
        }

        #endregion

        #region Public Properties

        public int TileID { get; private set; }

        public int Color { get; private set; }

        public int Counter { get; private set; }

        #endregion
    }
}