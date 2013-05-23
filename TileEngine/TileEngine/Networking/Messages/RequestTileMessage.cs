namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestTileMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public RequestTileMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public RequestTileMessage(int tileID, int color, int counter)
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

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.RequestTileState;
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
            this.TileID = im.ReadInt32();
            this.Color = im.ReadInt32();
            this.Counter = im.ReadInt32();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.TileID);
            om.Write(this.Color);
            om.Write(this.Counter);
        }

        #endregion
    }
}