using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lpIRC
{
    /// <summary>
    /// Holds all relevant IRC-Protocol constants, including most
    /// importantly message-IDS, error and server responses, etc.
    /// Refer to RFC 1459.
    /// </summary>
    public static class IRCConstants
    {
        /// <summary>
        /// Joins a channel.
        /// </summary>
        public const string MSG_JOIN = "JOIN";
        /// <summary>
        /// Server Ping. Must PONG or get kicked.
        /// </summary>
        public const string MSG_PING = "PING";
        /// <summary>
        /// Leaves a channel.
        /// </summary>
        public const string MSG_PART = "PART";
        /// <summary>
        /// Disconnects from server.
        /// </summary>
        public const string MSG_QUIT = "QUIT";
        /// <summary>
        /// Kicked from channel.
        /// </summary>
        public const string MSG_KICK = "KICK";
        /// <summary>
        /// Direct message to user or channel.
        /// </summary>
        public const string MSG_PRIVMSG = "PRIVMSG";
        /// <summary>
        /// Login-Success.
        /// </summary>
        public const string MSG_001 = "001";
        /// <summary>
        /// Answer to TOPIC-request.
        /// </summary>
        public const string MSG_332 = "332";
        /// <summary>
        /// Info on who set the topic and when.
        /// </summary>
        public const string MSG_333 = "333";
        /// <summary>
        /// NAMES request on channel.
        /// </summary>
        public const string MSG_353 = "353";
        /// <summary>
        /// End of a NAMES list from server.
        /// </summary>
        public const string MSG_366 = "366";
        /// <summary>
        /// No such channel error response.
        /// </summary>
        public const string MSG_403 = "403";
        /// <summary>
        /// NICK error response: nickname already in use.
        /// </summary>
        public const string MSG_433 = "433";
        /// <summary>
        /// JOIN-error response: channel is invite-only (MODE +i).
        /// </summary>
        public const string MSG_473 = "473";
        /// <summary>
        /// JOIN-error response: channel is key-only (MODE +k).
        /// </summary>
        public const string MSG_475 = "475";
        /// <summary>
        /// User changed his nickname.
        /// </summary>
        public const string MSG_NICK = "NICK";
        /// <summary>
        /// Channel-Topic was changed.
        /// </summary>
        public const string MSG_TOPIC = "TOPIC";
        /// <summary>
        /// Channel-Mode or user rights were changed.
        /// </summary>
        public const string MSG_MODE = "MODE";
    }
}
