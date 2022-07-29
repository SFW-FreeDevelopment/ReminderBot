using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using ReminderBot.App.Repositories;

namespace ReminderBot.App.Commands
{
    public class ListCommand : CommandBase
    {
        private readonly ReminderRepository _reminderRepository;
        
        public ListCommand(ReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        
        [Command("list")]
        public async Task HandleCommandAsync()
        {
            var reminders = await _reminderRepository.Get();

            if (reminders == null || !reminders.Any())
            {
                await ReplyAsync("There are no reminders stored.");
            }
            else
            {
                foreach (var reminder in reminders)
                {
                    await ReplyAsync($"Reminder {reminder.Id} at {reminder.RemindAt.ToLongDateString()} {reminder.RemindAt.ToShortTimeString()}: {reminder.Message}");
                }
            }
        }
    }
}