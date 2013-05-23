namespace TileEngine.Networking.Args
{
    using System;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public BannerArgs(string name, int color, bool isUsingAvatar,int pos)
        {
            this.Name = name;
            this.Color = color;
            this.IsUsingAvatar = isUsingAvatar;
            this.Pos = pos;
        }

        #endregion

        #region Public Properties
        
        public string Name { get; private set; }

        public int Color { get; private set; }

        public bool IsUsingAvatar { get; private set; }

        public int Pos { get; private set; }
        
        #endregion
    }
}