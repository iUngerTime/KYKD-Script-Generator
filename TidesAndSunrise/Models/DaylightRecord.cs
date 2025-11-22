using TidesAndSunrise.Enums;

namespace TidesAndSunrise.Models;

public class DaylightRecord
{
    public DateOnly Date { get; set; }
    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }
    public TimeOnly Duration { get; set; }
    public TimeOnly? Gain { get; set; }
    public TimeOnly? Loss { get; set; }
    public string Notes { get; set; }

    public string LessOrMoreString
    {
        get
        {
            if (Gain != null) return $"{Gain.Value.ToString("hh")} minutes and {Gain.Value.ToString("mm")} seconds more than yesterday";
            if (Loss != null) return $"{Loss.Value.ToString("hh")} minutes and {Loss.Value.ToString("mm")} seconds less than yesterday";
            return "N/A";
        }
    }

    public DaylightRecord(string month, string day, string sunrise, string sunset, string duration, string gain, string loss, string notes)
    {
        Date = DateOnly.Parse($"{month} {day}");
        Sunrise = TimeOnly.FromDateTime(DateTime.Parse(sunrise));
        Sunset = TimeOnly.FromDateTime(DateTime.Parse(sunset));
        Duration = TimeOnly.FromDateTime(DateTime.Parse(duration));
        Gain = string.IsNullOrWhiteSpace(gain) ? null : TimeOnly.FromDateTime(DateTime.Parse(gain));
        Loss = string.IsNullOrWhiteSpace(loss) ? null : TimeOnly.FromDateTime(DateTime.Parse(loss));
        Notes = notes;
    }

    public override string ToString()
    {
        return $"{Date:MMMM dd}, Sunrise: {Sunrise}, Sunset: {Sunset}, Duration: {Duration}, Gain: {Gain}, Loss: {Loss}, Notes: {Notes}";
    }

    public string TimeDifferenceString()
    {
        return $"Giving us {Duration.Hour} hours and {Duration.Minute} minutes of daylight; {LessOrMoreString}";
    }

    public string SunriseString(TimeOfDay timeOfDay, TimeWindow timeWindow)
    {
        // if the sunrise is before the range
        if (Sunrise < timeWindow.Start)
        {
            return $"The sun rose this morning in Bethel at {Sunrise}";
        }
        // If the sunrise is in the range
        else if (timeWindow.Start <= Sunrise && Sunrise <= timeWindow.End)
        {
            return $"Sunrise this morning in Bethel, at {Sunrise}";
        }
        // if the sunrise is after the range
        else
        {
            return $"The sun will rise this morning at {Sunrise}.";
        }
    }

    public string SunsetString(TimeOfDay timeOfDay, TimeWindow timeWindow)
    {
        // if the sunset is before the range
        if (Sunset < timeWindow.Start)
        {
            return $"And the sun set at {Sunset}";
        }
        // If the suset is in the range
        else if (timeWindow.Start <= Sunset && Sunset <= timeWindow.End)
        {
            return $"And Sunset, at {Sunset}";
        }
        // if the sunset is after the range
        else
        {
            return $"And it will set this evening at {Sunset}";
        }
    }
}
