namespace TileEngine.Networking.Args
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateEntityArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public UpdateEntityArgs(int id,string name,int color, Vector2 location,float scale)
        {
            this.ID = id;
            this.Name = name;
            this.Color = color;
            this.Location = location;
            this.Scale = scale;
        }

        #endregion

        #region Public Properties

        public int ID { get; private set; }

        public string Name { get; private set; }

        public int Color { get; private set; }

        public Vector2 Location { get; private set; }

        public float Scale { get; private set; }

        #endregion
    }
}