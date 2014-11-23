﻿using System;
using System.Collections.Generic;

namespace ChatLe.Models
{
    public class Conversation
    {
        public Conversation()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public virtual string Id { get; set; }
        public virtual ICollection<IApplicationUser> Users { get; } = new List<IApplicationUser>();
        public virtual ICollection<Message> Messages { get; } = new List<Message>();
    }
}