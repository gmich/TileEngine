namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SnapTileMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public SnapTileMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public SnapTileMessage(int id, string name, int color)
        {
            this.ID = id;
            this.Name = name;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        public int ID { get; private set; }

        public string Name { get; private set; }

        public int Color { get; private set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.SnapTileState;
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
            this.ID = im.ReadInt32();
            this.Name = im.ReadString();
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
            om.Write(this.ID);
            om.Write(this.Name);
            om.Write(this.Color);
        }

        #endregion
    }
}