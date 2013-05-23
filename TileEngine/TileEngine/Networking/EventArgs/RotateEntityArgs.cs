namespace TileEngine.Networking.Args
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RotateEntityArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public RotateEntityArgs(int id, bool clockwise, string name, int color)
        {
            this.ID = id;
            this.Clockwise = clockwise;
            this.Name = name;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        public int ID { get; private set; }

        public bool Clockwise { get; private set; }

        public string Name { get; private set; }
        
        public int Color { get; private set; }

        #endregion
    }
}