using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;
using System.Text;

namespace GeneratoreCore.Builders;

public static class DaylightLineBuilder
{
	public static string BuildSunrise(DaylightRecord record, TimeOfDay timeOfDay)
	{
		TimeOnly sunrise = record.Sunrise;

		var morningWindow = TimeWindowCalculator.GetTimeWindow(record.Date, timeOfDay);
		var morningStart = morningWindow.Start;
		var morningEnd = morningWindow.End;

		if (sunrise < morningStart)
		{
			return $"The sun rose this morning in Bethel at {FormatTime12Hour(sunrise)}.";
		}

		if (sunrise >= morningStart && sunrise <= morningEnd)
		{
			return $"Sunrise this morning in Bethel, {FormatTime12Hour(sunrise)}.";
		}

		// Edge case: sunrise after window end (rare) – treat as future tense.
		return $"The sun will rise later this morning at {FormatTime12Hour(sunrise)}.";
	}

	public static string BuildSunset(DaylightRecord record, TimeOfDay timeOfDay)
	{
		TimeOnly sunset = record.Sunset;

		var eveningWindow = TimeWindowCalculator.GetTimeWindow(record.Date, timeOfDay);
		var eveningStart = eveningWindow.Start;
		var eveningEnd = eveningWindow.End;

		// After midnight case (00:XX)
		if (sunset.Hour == 0)
		{
			return $"And the sun will set {sunset.Minute} minutes after midnight.";
		}

		if (sunset < eveningStart)
		{
			return $"And the sun set in Bethel this afternoon/evening at {FormatTime12Hour(sunset)}.";
		}

		if (sunset >= eveningStart && sunset <= eveningEnd)
		{
			return $"And sunset time in Bethel, {FormatTime12Hour(sunset)}.";
		}

		if (sunset > eveningEnd && sunset.Hour < 24)
		{
			return $"And the sun will set this evening at {FormatTime12Hour(sunset)}.";
		}

		// Fallback
		return $"And Sunset in Bethel at {FormatTime12Hour(sunset)}.";
	}

	/// <summary>
	/// Based on the current time of day, give a tense-appropriate daylight length line.
	/// </summary>
	public static string BuildDaylightLength(DaylightRecord record, TimeOfDay timeOfDay)
	{
		TimeOnly sunrise = record.Sunrise;
		TimeOnly sunset = record.Sunset;
		TimeOnly duration = record.Duration;

		int hours = duration.Hour;
		int minutes = duration.Minute;

		var window = TimeWindowCalculator.GetTimeWindow(record.Date, timeOfDay);
		var windowStart = window.Start;
		var windowEnd = window.End;

		string? sunDuration = CreateDurationString(record);

		// Determine tense based on sunset relative to evening window.
		if (sunset < windowStart)
		{
			return $"That gave us {hours} hours {minutes} minutes of daylight. {sunDuration}";
		}

		if (sunset >= windowStart && sunset <= windowEnd)
		{
			return $"Giving us {hours} hours {minutes} minutes of daylight. {sunDuration}";
		}

		// Includes sunset after window or after midnight
		return $"That will give us {hours} hours {minutes} minutes of daylight. {sunDuration}";
	}

	private static string FormatTime12Hour(TimeOnly time)
	{
		return time.ToString("h:mm tt").ToLower(); // e.g., "6:08 am"
	}

	private static string CreateDurationString(DaylightRecord record)
	{
		int minutes = record.Duration.Minute;
		int seconds = record.Duration.Second;

		StringBuilder duration = new StringBuilder();

		if (minutes > 0)
		{
			duration.Append($"{minutes} minute{(minutes > 1 ? "s" : string.Empty)}");
		}

		if (seconds > 0)
		{
			if (duration.Length > 0)
			{
				duration.Append(" and ");
			}
			duration.Append($"{seconds} second{(seconds > 1 ? "s" : string.Empty)}");
		}

		if (duration.Length > 0)
		{
			duration.Append($" {record.ChangeQuantifier} than yesterday.");
		}

		return duration.ToString();
	}
}
