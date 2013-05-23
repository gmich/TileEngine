namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AddPointingAnimationMessage : IGameMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBannerMessage"/> class.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public AddPointingAnimationMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public AddPointingAnimationMessage(Vector2 location, int color, float scale)
        {
            this.Location = location;
            this.Color = color;
            this.Scale = scale;
        }

        #endregion

        #region Public Properties

        public Vector2 Location { get; private set; }

        public int Color { get; private set; }

        public float Scale { get; private set; }

        /// <summary>
        /// Gets MessageType.
        /// </summary>
        /// 
        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.AddPointingAnimationState;
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
            this.Location = im.ReadVector2();
            this.Color = im.ReadInt32();
            this.Scale = im.ReadFloat();
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Location);
            om.Write(this.Color);
            om.Write(this.Scale);
        }

        #endregion
    }
}