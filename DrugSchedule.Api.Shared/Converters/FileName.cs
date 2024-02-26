using DrugSchedule.Api.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrugSchedule.Api.Shared.Converters;

public class RepeatDayOfWeekDtoConverter : JsonConverter<RepeatDayOfWeekDto>
{
    private static readonly string[] Days = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"];

    public override RepeatDayOfWeekDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Unexpected token type.");
        }

        var daysOfWeekStrings = JsonSerializer.Deserialize<string[]>(ref reader, options);
        if (daysOfWeekStrings.Length > Days.Length)
        {
            throw new JsonException("Max 7 items in array");
        }

        RepeatDayOfWeekDto result = 0;
        foreach (var dayOfWeekString in daysOfWeekStrings)
        {
            var index = Array.IndexOf(Days, dayOfWeekString.ToLower());
            if (index < 0)
            {
                throw new JsonException($"\"{dayOfWeekString}\" is not valid three-letter day name");
            }

            result |= (RepeatDayOfWeekDto)(1 << index);
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, RepeatDayOfWeekDto value, JsonSerializerOptions options)
    {
        List<string> daysOfWeekStrings = new List<string>();
        var intValue = (int)value;
        for (int d = 0; d < 7; d++)
        {
            if ((intValue & (1 << d)) != 0)
            {
                daysOfWeekStrings.Add(Days[1 << d]);
            }
        }

        JsonSerializer.Serialize(writer, daysOfWeekStrings, options);
    }
}