using System.Threading.Tasks;
using Discord.Commands;
using ReminderBot.App.Models;
using ReminderBot.App.Repositories;

namespace ReminderBot.App.Commands
{
    public class AddCommand : CommandBase
    {
        private readonly ReminderRepository _reminderRepository;
        
        public AddCommand(ReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        
        [Command("add")]
        public async Task HandleCommandAsync(int value, string interval, [Remainder] string message)
        {
            if (value <= 0)
            {
                await ReplyAsync("Value must be greater than 0.");
                return;
            }
            
            var reminder = new Reminder
            {
                Context = Context,
                Message = message
            };

            switch (interval.ToLower())
            {
                case "second":
                case "seconds":
                    reminder.RemindAt = reminder.CreatedAt.AddSeconds(value);
                    break;
                case "minute":
                case "minutes":
                    reminder.RemindAt = reminder.CreatedAt.AddMinutes(value);
                    break;
                case "hour":
                case "hours":
                    reminder.RemindAt = reminder.CreatedAt.AddHours(value);
                    break;  
                case "day":
                case "days":
                    reminder.RemindAt = reminder.CreatedAt.AddDays(value);
                    break;
                case "week":
                case "weeks":
                    reminder.RemindAt = reminder.CreatedAt.AddDays(value * 7);
                    break;
                case "month":
                case "months":
                    reminder.RemindAt = reminder.CreatedAt.AddMonths(value);
                    break;
                default:
                    await ReplyAsync("Invalid interval.");
                    return;
            }
            
            await _reminderRepository.Add(reminder);

            await ReplyAsync($"Reminder set for {reminder.RemindAt.ToLongDateString()} {reminder.RemindAt.ToShortTimeString()}.");
        }
    }
}