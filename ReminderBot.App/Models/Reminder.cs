using System;
using Discord.Commands;
using Discord.WebSocket;

namespace ReminderBot.App.Models
{
    public class Reminder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime RemindAt { get; set; }
        public bool HasTriggered { get; set; }
        public SocketCommandContext Context { get; set; }
    }
}