using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;
using GeneratoreCore.Builders;

namespace GeneratoreCore;

public interface IScriptDataProvider
{
	public ScriptContext Build(DateOnly date, TimeOfDay timeOfDay);
}

public class DefaultScriptDataProvider : IScriptDataProvider
{
	public ScriptContext Build(DateOnly date, TimeOfDay timeOfDay)
	{
		int month = date.Month;
		int day = date.Day;

		var daylight = ParseDaylight.GetInstance().GetDaylightInfoForDay(month, day);
		var window = TimeWindowCalculator.GetTimeWindowForDate(month, day, timeOfDay);

		// Build daylight lines
		string sunrise = DaylightLineBuilder.BuildSunrise(daylight, timeOfDay);
		string sunset = DaylightLineBuilder.BuildSunset(daylight, timeOfDay);
		string daylightLength = DaylightLineBuilder.BuildDaylightLength(daylight);

		// Village tide lines
		string bethel = BuildVillage(Village.Bethel, timeOfDay, month, day, window);
		string quinhagak = BuildVillage(Village.Quinhagak, timeOfDay, month, day, window);
		string togiak = BuildVillage(Village.Togiak, timeOfDay, month, day, window);

		return new ScriptContext
		{
			PeriodLabel = timeOfDay.ToShortString(),
			DayOfWeek = date.DayOfWeek.ToString(),
			Month = date.ToString("MMMM"),
			Day = day,
			SunriseLine = sunrise,
			SunsetLine = sunset,
			DaylightLine = daylightLength,
			BethelTideLine = bethel,
			QuinhagakTideLine = quinhagak,
			TogiakTideLine = togiak
		};
	}

	private string BuildVillage(Village village, TimeOfDay timeOfDay, int month, int day, TimeWindow window)
	{
		var tides = ParseTides.GetInstance().GetTideDetailsForDay(village, timeOfDay, month, day);
		return VillageTideLineBuilder.AggregateLines(tides, village, window);
	}
}
