using TidesAndSunrise.Enums;
using TidesAndSunrise.Models;

namespace TidesAndSunrisesScriptGenerator.Helpers;

static class TimeWindowCalculator
{
    static public TimeWindow GetTimeWindowForDate(int month, int day, TimeOfDay timeOfDay)
    {
        // Create a DateOnly object for the given date
        DateOnly date = new DateOnly(2025, month, day);
        TimeWindow timeWindow = GetWindowOfTime(date.DayOfWeek, timeOfDay);

        return timeWindow;
    }

    static private TimeWindow GetWindowOfTime(DayOfWeek dayOfWeek, TimeOfDay timeOfDay)
    {
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
