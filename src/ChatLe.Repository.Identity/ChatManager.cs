﻿using Microsoft.Framework.OptionsModel;

namespace ChatLe.Models
{
    /// <summary>
    /// Default <see cref="ChatManager<TKey, TUser, TConversation, TAttendee, TMessage, TNotificationConnection>"/>
    /// </summary>
    public class ChatManager : ChatManager<string, ChatLeUser, Conversation, Attendee, Message, NotificationConnection>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">a chat store</param>
        public ChatManager(IChatStore<string, ChatLeUser, Conversation, Attendee, Message, NotificationConnection> store, IOptions<ChatOptions> options) : base(store, options) { }
    }    
}