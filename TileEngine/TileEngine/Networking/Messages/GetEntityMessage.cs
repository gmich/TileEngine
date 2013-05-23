namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetEntityMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public GetEntityMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public GetEntityMessage(int entityID, int infoID, int senderColor)
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

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.GetEntityState;
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
            this.EntityID = im.ReadInt32();
            this.InfoID = im.ReadInt32();
            this.SenderColor = im.ReadInt32();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.EntityID);
            om.Write(this.InfoID);
            om.Write(this.SenderColor);
        }

        #endregion
    }
}