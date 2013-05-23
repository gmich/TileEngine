namespace TileEngine.Networking.Args
{
    using System;
    using Microsoft.Xna.Framework;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeEntityArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public ChangeEntityArgs(int entityID, int infoID, int senderColor)
        {
            this.EntityID = entityID;
            this.InfoID = infoID;
            this.SenderColor = senderColor;
        }

        #endregion

        #region Public Properties

        public int EntityID { get; private set; }

        public int InfoID { get; private set; }

        public int SenderColor { get; private set; }

        #endregion
    }
}