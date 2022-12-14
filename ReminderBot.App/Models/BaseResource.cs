using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ReminderBot.App.Models;

public abstract class BaseResource
{
    [Required, BsonId] public string Id { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [Required] public DateTime UpdatedAt { get; set; }
    [Required, ConcurrencyCheck] public int Version { get; set; }

    protected BaseResource()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Version = 1;
    }
}