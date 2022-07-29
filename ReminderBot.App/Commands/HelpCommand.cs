using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace ReminderBot.App.Commands
{
    public class HelpCommand : CommandBase
    {
        [Command("help")]
        public async Task HandleCommandAsync()
        {
            await ReplyAsync($"**The following commands can be used:**{Environment.NewLine}" +
                             $"  • **ping** - Pings the Discord channel{Environment.NewLine}" +
                             $"  • **konami** - Displays the Konami code as emojis{Environment.NewLine}" +
                             $"  • **add 1 minute \"Your message here\"** - Adds a reminder at the specified interval{Environment.NewLine}" +
                             $"  • **list** - Lists all currently queued reminders{Environment.NewLine}");
        }
    }
}