namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TitleMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public TitleMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public TitleMessage(string title, bool show)
        {
            this.Title = title;
            this.Show = show;
        }

        #endregion

        #region Public Properties

        public string Title { get; private set; }

        public bool Show { get; private set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.SetTitleState;
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
            this.Title = im.ReadString();
            this.Show = im.ReadBoolean();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Title);
            om.Write(this.Show);
        }

        #endregion
    }
}