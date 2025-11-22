using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

namespace GeneratoreCore.Builders;

public static class DaylightLineBuilder
{
	public static string BuildSunrise(DaylightRecord record, TimeOfDay timeOfDay)
	{
		TimeWindow timeWindow = TimeWindowCalculator.GetTimeWindow(record.Date, timeOfDay);
		TimeOnly sunrise = record.Sunrise;

		return $"Sunrise: {sunrise:HH:mm} ({timeWindow})";
	}

	public static string BuildSunset(DaylightRecord record, TimeOfDay timeOfDay)
	{
		TimeWindow timeWindow = TimeWindowCalculator.GetTimeWindow(record.Date, timeOfDay);
		TimeOnly sunset = record.Sunset;

		return $"Sunset: {sunset:HH:mm} ({timeWindow})";
	}

	public static string BuildDaylightLength(DaylightRecord record)
	{
		TimeSpan length = record.Sunset - record.Sunrise;

		return $"Daylight Length: {length.Hours}h {length.Minutes}m";
	}
}
