﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace ChatLe.Models
{
    public class ApplicationUser : IdentityUser, IApplicationUser<string>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public ApplicationUser(string userName) :this()
        {
            UserName = userName;
        }
        public virtual bool IsConnected { get; set; }
        public virtual string SignalRConnectionId { get; set; }
    }
}