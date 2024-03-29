﻿using DrugSchedule.Services.Models;
using DrugSchedule.Services.Models.Schedule;
using DrugSchedule.StorageContract.Data;
using TakingScheduleExtended = DrugSchedule.StorageContract.Data.TakingScheduleExtended;
using UserContactSimple = DrugSchedule.StorageContract.Data.UserContactSimple;

namespace DrugSchedule.Services.Converters;

public interface IScheduleConverter
{
    Models.TakingСonfirmation ToConfirmation(TakingСonfirmationExtended confirmation);
    Models.TakingScheduleExtended ToScheduleExtended(TakingScheduleExtended schedule);
    ScheduleSimple ToScheduleSimple(TakingScheduleSimple schedule);
    ScheduleSimpleCollection ToScheduleSimpleCollection(List<TakingScheduleSimple> scheduleList);
    ScheduleExtendedCollection ToScheduleExtendedCollection(List<TakingScheduleExtended> scheduleList);
    TakingСonfirmationCollection ToСonfirmationCollection(List<TakingСonfirmationExtended> confirmationsList);
}