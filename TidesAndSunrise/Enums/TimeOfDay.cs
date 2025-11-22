namespace TidesAndSunrise.Enums
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening,
    }

    public static class TimeOfDayExtensions
    {
        public static string ToShortString(this TimeOfDay timeOfDay)
        {
            return timeOfDay switch
            {
                TimeOfDay.Morning => "M",
                TimeOfDay.Afternoon => "A",
                TimeOfDay.Evening => "E",
                _ => throw new ArgumentOutOfRangeException(nameof(timeOfDay), timeOfDay, null)
            };
        }

        public static string GreetingString(this TimeOfDay timeOfDay)
        {
            return timeOfDay switch
            {
                TimeOfDay.Morning => "Good morning",
                TimeOfDay.Afternoon => "Good afternoon",
                TimeOfDay.Evening => "Good evening",
                _ => throw new ArgumentOutOfRangeException(nameof(timeOfDay), timeOfDay, null)
            };
        }

        public static string NameString(this TimeOfDay timeOfDay)
        {
            return timeOfDay switch
            {
                TimeOfDay.Morning => "morning",
                TimeOfDay.Afternoon => "afternoon",
                TimeOfDay.Evening => "evening",
                _ => throw new ArgumentOutOfRangeException(nameof(timeOfDay), timeOfDay, null)
            };
        }
    }
}
