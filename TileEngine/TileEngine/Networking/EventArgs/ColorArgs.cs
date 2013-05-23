namespace TileEngine.Networking.Args
{
    using System;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ColorArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public ColorArgs(string name, int color, int oldColor,int pos)
        {
            this.Name = name;
            this.Color = color;
            this.OldColor = oldColor;
            this.Pos = pos;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        public int Color { get; private set; }

        public int OldColor { get; private set; }

        public int Pos { get; private set; }

        #endregion
    }
}