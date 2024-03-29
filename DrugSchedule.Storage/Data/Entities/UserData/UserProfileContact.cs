﻿namespace DrugSchedule.Storage.Data.Entities;

public class UserProfileContact
{
    public long Id { get; set; }

    public required long UserProfileId { get; set; }

    public UserProfile? UserProfile { get; set; }

    public required long ContactProfileId { get; set; }

    public UserProfile? ContactProfile { get; set; }

    public required string CustomName { get; set; }

    public List<ScheduleShare> ScheduleShares { get; set; } = new();
}