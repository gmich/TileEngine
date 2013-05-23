namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateScoreMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public UpdateScoreMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public UpdateScoreMessage(int bannerID, int score, int color)
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

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.UpdateScoreState;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The decode.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public void Decode(NetIncomingMessage im)
        {
            this.BannerID = im.ReadInt32();
            this.Score = im.ReadInt32();
            this.Color = im.ReadInt32();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.BannerID);
            om.Write(this.Score);
            om.Write(this.Color);
        }

        #endregion
    }
}