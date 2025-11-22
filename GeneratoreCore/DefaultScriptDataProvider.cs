using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;
using GeneratoreCore.Builders;
using GeneratoreCore.Parsers;

namespace GeneratoreCore;

/// <summary>
/// The default provider parses tide and daylight data from Excel files
/// located in the global resources directory.
/// </summary>
public class DefaultScriptDataProvider : IScriptDataProvider
{
	public ScriptContext Build(DateOnly date, TimeOfDay timeOfDay)
	{
		int month = date.Month;
		int day = date.Day;

		DaylightRecord daylight = DaylightParser.GetInstance().GetDaylightInfoForDay(month, day);
		TimeWindow window = TimeWindowCalculator.GetTimeWindow(date, timeOfDay);

		// Build daylight lines
		string sunrise = DaylightLineBuilder.BuildSunrise(daylight, timeOfDay);
		string sunset = DaylightLineBuilder.BuildSunset(daylight, timeOfDay);
		string daylightLength = DaylightLineBuilder.BuildDaylightLength(daylight, timeOfDay);

		// Village tide lines
		string bethel = BuildVillageTideInfo(Village.Bethel, date, timeOfDay, window);
		string quinhagak = BuildVillageTideInfo(Village.Quinhagak, date, timeOfDay, window);
		string togiak = BuildVillageTideInfo(Village.Togiak, date, timeOfDay, window);

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

	private string BuildVillageTideInfo(Village village, DateOnly date, TimeOfDay timeOfDay, TimeWindow window)
	{
		List<TideInfo> tides = TideParser.GetInstance().GetTideDetailsForDate(village, date, timeOfDay);
		return VillageTideLineBuilder.AggregateLines(tides, village, window);
	}
}
