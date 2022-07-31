using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ReminderBot.App.Models;

namespace ReminderBot.App.Repositories
{
    public class ReminderRepository
    {
        private static readonly Dictionary<string, Reminder> _reminders = new Dictionary<string, Reminder>();
        
        private readonly IMongoClient _mongoClient;
        
        public ReminderRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }
        
        public async Task<ICollection<Reminder>> Get()
        {
            return await GetCollection()
                .AsQueryable()
                .Where(x => !x.HasTriggered)
                .ToListAsync();
        }

        public async Task<Reminder> Add(Reminder data)
        {
            data.Id = Guid.NewGuid().ToString();
            data.Version = 1;
            data.CreatedAt = DateTime.UtcNow;
            data.UpdatedAt = data.CreatedAt;
            await GetCollection().InsertOneAsync(data);
            return await GetCollection().AsQueryable()
                .FirstOrDefaultAsync(x => x.Id.Equals(data.Id));
        }
        
        private IMongoCollection<Reminder> GetCollection()
        {
            var database = _mongoClient.GetDatabase("ReminderBot");
            var collection = database.GetCollection<Reminder>("Reminders");
            return collection;
        }
    }
}