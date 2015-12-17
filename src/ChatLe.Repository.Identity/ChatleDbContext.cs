﻿using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace ChatLe.Models
{
    public class ChatLeIdentityDbContextSql : ChatLeIdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=chatle;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(options);
        }
    }

    public class ChatLeIdentityDbContext:ChatLeIdentityDbContext<string, Message, Attendee, Conversation, NotificationConnection>
    { }
    /// <summary>
    /// Database context for ChatLe user
    /// </summary>
    public class ChatLeIdentityDbContext<TKey, TMessage, TAttendee, TConversation, TNotificationConnection> : IdentityDbContext<ChatLeUser> 
        where TKey: IEquatable<TKey>
        where TMessage : Message<TKey>
        where TAttendee : Attendee<TKey>
        where TConversation : Conversation<TKey>
        where TNotificationConnection : NotificationConnection<TKey>
    {
        /// <summary>
        /// Gets or sets the DbSet of messages
        /// </summary>
        public DbSet<TMessage> Messages { get; set; }
        /// <summary>
        /// Gets or sets the DbSet of conversations
        /// </summary>
        public DbSet<TConversation> Conversations { get; set; }
        /// <summary>
        /// Gets or sets the DbSet of attendees
        /// </summary>
        public DbSet<TAttendee> Attendees { get; set; }
        /// <summary>
        /// Gets or sets the DbSet of notification connections
        /// </summary>
        public DbSet<TNotificationConnection> NotificationConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatLeUser>()
                .HasMany(u => u.NotificationConnections)
                .WithOne()
                .HasForeignKey(nc => new { nc.ConnectionId, nc.NotificationType })
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NotificationConnection<TKey>>(b =>
            {
                b.HasKey(n => new { n.ConnectionId, n.NotificationType });
                b.ToTable("NotificationConnections");
            });


            builder.Entity<Conversation<TKey>>(b =>
            {
                b.HasKey(c => c.Id);
                b.ToTable("Conversations");

                b.HasMany(c => c.Attendees)
                    .WithOne()
                    .HasForeignKey(a => a.ConversationId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(c => c.Messages)
                    .WithOne()
                    .HasForeignKey(m => m.ConversationId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Message<TKey>>(b =>
            {
                b.HasKey(m => m.Id);
                b.ToTable("Messages");
            });

            builder.Entity<Attendee<TKey>>(b =>
            {
                b.HasKey(a => new { a.ConversationId, a.UserId });
                b.ToTable("Attendees");                
            });            
        }
    }
}