using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using ReminderBot.App.Repositories;
// ReSharper disable FunctionNeverReturns

namespace ReminderBot.App.Services
{
    public class ReminderService
    {
        /// Time between checks in milliseconds
        private const int TIME_BETWEEN_CHECKS = 6000;
        
        private readonly ReminderRepository _reminderRepository;
        
        public ReminderService(ReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task CheckReminders()
        {
            while (true)
            {
                await Task.Delay(TIME_BETWEEN_CHECKS);
                
                var reminders = await _reminderRepository.Get();
                var orderedReminders = reminders.OrderBy(x => x.RemindAt);

                foreach (var reminder in orderedReminders)
                {
                    if (DateTime.Now >= reminder.RemindAt)
                    {
                        var message = $"{reminder.Context.User.Mention} {reminder.Message}";
                        await reminder.Context.Channel.SendMessageAsync(message).ConfigureAwait(false);
                        reminder.HasTriggered = true;
                        continue;
                    }
                    break;
                }
            }
        }
    }
}