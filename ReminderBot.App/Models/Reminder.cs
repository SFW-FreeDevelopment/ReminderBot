using System;
using Discord.Commands;

namespace ReminderBot.App.Models
{
    public class Reminder : BaseResource
    {
        public string Message { get; set; }
        public DateTime RemindAt { get; set; }
        public bool HasTriggered { get; set; }
        public SocketCommandContext Context { get; set; }
    }
}