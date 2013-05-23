namespace TileEngine.Networking.Args
{
    using System;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public ScoreArgs(int bannerID, int score, int color)
        {
            this.BannerID = bannerID;
            this.Score = score;
            this.Color = color;
        }

        #endregion

        #region Public Properties
        
        public int BannerID { get; private set; }

        public int Score { get; private set; }

        public int Color { get; private set; }

        #endregion
    }
}