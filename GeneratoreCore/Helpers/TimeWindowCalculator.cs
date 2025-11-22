using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

namespace GeneratorCore.Helpers;

static class TimeWindowCalculator
{
	/// <summary>
	/// Returns the time window corresponding to the specified date and time of day.
	/// </summary>
	/// <param name="date">The date for which to retrieve the time window. The day of the week is determined from this value.</param>
	/// <param name="timeOfDay">The time of day category (such as morning, afternoon, or evening) for which to retrieve the time window.</param>
	/// <returns>A TimeWindow instance representing the start and end times for the specified date and time of day.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the specified combination of date and time of day does not correspond to a defined time window.</exception>
	static public TimeWindow GetTimeWindow(DateOnly date, TimeOfDay timeOfDay)
	{
		DayOfWeek dayOfWeek = date.DayOfWeek;

		switch (dayOfWeek)
		{
			case DayOfWeek.Monday:
			case DayOfWeek.Tuesday:
			case DayOfWeek.Wednesday:
			case DayOfWeek.Thursday:
			case DayOfWeek.Friday:
			case DayOfWeek.Saturday:
			case DayOfWeek.Sunday:
				// This is the current instructions based on 11/22/2025
				switch (timeOfDay)
				{
					case TimeOfDay.Morning:
						return new TimeWindow(new TimeOnly(7, 08), new TimeOnly(11, 08));
					case TimeOfDay.Afternoon:
						return new TimeWindow(new TimeOnly(12, 00), new TimeOnly(1, 00));
					case TimeOfDay.Evening:
						return new TimeWindow(new TimeOnly(12 + 7, 08), new TimeOnly(12 + 11, 08));
				}
				break;
		}

		throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "No matching time window found.");
	}
}
