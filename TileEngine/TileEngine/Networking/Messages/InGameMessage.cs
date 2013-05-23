namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InGameMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public InGameMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public InGameMessage(bool play)
        {
            this.Play = play;
        }

        #endregion

        #region Public Properties
    
        public bool Play { get; private set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.InGameState;
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
            this.Play = im.ReadBoolean();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Play);
        }

        #endregion
    }
}