namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerDisconnectedMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public PlayerDisconnectedMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public PlayerDisconnectedMessage(string name, int color, bool isUsingAvatar, int pos)
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

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.PlayerDisconnectedState;
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
            this.Name = im.ReadString();
            this.Color = im.ReadInt32();
            this.IsUsingAvatar = im.ReadBoolean();
            this.Pos = im.ReadInt32();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Name);
            om.Write(this.Color);
            om.Write(this.IsUsingAvatar);
            om.Write(this.Pos);
        }

        #endregion
    }
}