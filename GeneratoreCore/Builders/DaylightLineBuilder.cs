using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

namespace GeneratoreCore.Builders;

public static class DaylightLineBuilder
{
	public static string BuildSunrise(DaylightRecord record, TimeOfDay timeOfDay)
	{
		var timeWindow = TimeWindowCalculator.GetTimeWindowForDate(record.Date.Month, record.Date.Day, timeOfDay);
		var sunrise = record.Sunrise;
		// Example logic: format sunrise with time window
		return $"Sunrise: {sunrise:HH:mm} ({timeWindow})";
	}

	public static string BuildSunset(DaylightRecord record, TimeOfDay timeOfDay)
	{
		var timeWindow = TimeWindowCalculator.GetTimeWindowForDate(record.Date.Month, record.Date.Day, timeOfDay);
		var sunset = record.Sunset;
		// Example logic: format sunset with time window
		return $"Sunset: {sunset:HH:mm} ({timeWindow})";
	}

	public static string BuildDaylightLength(DaylightRecord record)
	{
		var length = record.Sunset - record.Sunrise;
		// Example logic: format time difference
		return $"Daylight Length: {length.Hours}h {length.Minutes}m";
	}
}
