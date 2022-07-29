using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReminderBot.App.Models;

namespace ReminderBot.App.Repositories
{
    public class ReminderRepository
    {
        private static readonly Dictionary<string, Reminder> _reminders = new Dictionary<string, Reminder>();

        public async Task<ICollection<Reminder>> Get()
        {
            return await Task.FromResult(_reminders.Values.Where(x => !x.HasTriggered).ToList());
        }
        
        public async Task<Reminder> Get(string id)
        {
            return _reminders.TryGetValue(id, out var reminder)
                ? await Task.FromResult(reminder)
                : await Task.FromResult((Reminder)null);
        }

        public async Task Add(Reminder reminder)
        {
            _reminders.Add(reminder.Id.ToString(), reminder);
            await Task.CompletedTask;
        }
    }
}