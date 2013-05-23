namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RotateEntityMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public RotateEntityMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public RotateEntityMessage(int id, bool clockwise, string name, int color)
        {
            this.ID = id;
            this.Clockwise = clockwise;
            this.Name = name;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        public int ID { get; private set; }

        public bool Clockwise { get; private set; }

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
                return GameMessageTypes.RotateEntityState;
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
            this.Clockwise = im.ReadBoolean();
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
            om.Write(this.Clockwise);
            om.Write(this.Name);
            om.Write(this.Color);
        }

        #endregion
    }
}