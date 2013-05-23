namespace TileEngine.Networking.Args
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SignalEntityArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public SignalEntityArgs(int id,string name,int color)
        {
            this.ID = id;
            this.Name = name;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        public int ID { get; private set; }

        public string Name { get; private set; }

        public int Color { get; private set; }

        #endregion
    }
}