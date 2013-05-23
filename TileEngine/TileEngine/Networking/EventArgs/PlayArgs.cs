namespace TileEngine.Networking.Args
{
    using System;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public PlayArgs(bool play)
        {
            this.Play = play;
        }

        #endregion

        #region Public Properties

        public bool Play { get; private set; }
                
        #endregion
    }
}