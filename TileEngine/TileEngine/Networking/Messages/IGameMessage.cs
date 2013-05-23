﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGameMessage.cs" company="">
//   
// </copyright>
// <summary>
//   The i game message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TileEngine.Networking.Messages
{
    using Lidgren.Network;

    /// <summary>
    /// The i game message.
    /// </summary>
    public interface IGameMessage
    {
        #region Public Properties

        GameMessageTypes MessageType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The decode.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        void Decode(NetIncomingMessage im);

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        void Encode(NetOutgoingMessage om);

        #endregion
    }
}