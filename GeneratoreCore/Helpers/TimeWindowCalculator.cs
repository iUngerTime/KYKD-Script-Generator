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
				switch (timeOfDay)
				{
					case TimeOfDay.Morning:
						return new TimeWindow(new TimeOnly(6, 08), new TimeOnly(9, 10));
					case TimeOfDay.Afternoon:
						return new TimeWindow(new TimeOnly(13, 08), new TimeOnly(16, 10));
					case TimeOfDay.Evening:
						return new TimeWindow(new TimeOnly(21, 07), new TimeOnly(21, 09));
				}
				break;

			case DayOfWeek.Tuesday:
			case DayOfWeek.Wednesday:
			case DayOfWeek.Thursday:
			case DayOfWeek.Friday:
				switch (timeOfDay)
				{
					case TimeOfDay.Morning:
						return new TimeWindow(new TimeOnly(6, 08), new TimeOnly(9, 10));
					case TimeOfDay.Afternoon:
						return new TimeWindow(new TimeOnly(13, 08), new TimeOnly(16, 10));
					case TimeOfDay.Evening:
						return new TimeWindow(new TimeOnly(18, 07), new TimeOnly(21, 09));
				}
				break;

			case DayOfWeek.Saturday:
				switch (timeOfDay)
				{
					case TimeOfDay.Morning:
						return new TimeWindow(new TimeOnly(6, 08), new TimeOnly(10, 10));
					case TimeOfDay.Afternoon:
						return new TimeWindow(new TimeOnly(13, 08), new TimeOnly(14, 10));
					case TimeOfDay.Evening:
						return new TimeWindow(new TimeOnly(20, 08), new TimeOnly(21, 10));
				}
				break;

			case DayOfWeek.Sunday:
				switch (timeOfDay)
				{
					case TimeOfDay.Morning:
						return new TimeWindow(new TimeOnly(6, 07), new TimeOnly(9, 10));
					case TimeOfDay.Afternoon:
						return new TimeWindow(new TimeOnly(14, 08), new TimeOnly(16, 10));
					case TimeOfDay.Evening:
						return new TimeWindow(new TimeOnly(18, 08), new TimeOnly(22, 10));
				}
				break;
		}

		throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "No matching time window found.");
	}
}
