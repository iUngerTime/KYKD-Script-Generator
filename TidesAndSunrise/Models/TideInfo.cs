using TidesAndSunrise.Enums;

namespace TidesAndSunrise.Models;

public class TideInfo
{
	public DateOnly Date { get; set; }
	public DayOfWeek Day { get; set; }
	public TimeOnly Timeframe { get; set; }
	public string Type { get; set; }
	public double Height { get; set; }

	public TideInfo(string date, string day, string type, string height, string timeframe)
	{
		switch (day)
		{
			case "Mon":
				Day = DayOfWeek.Monday;
				break;
			case "Tue":
				Day = DayOfWeek.Tuesday;
				break;
			case "Wed":
				Day = DayOfWeek.Wednesday;
				break;
			case "Thu":
				Day = DayOfWeek.Thursday;
				break;
			case "Fri":
				Day = DayOfWeek.Friday;
				break;
			case "Sat":
				Day = DayOfWeek.Saturday;
				break;
			case "Sun":
				Day = DayOfWeek.Sunday;
				break;
			default:
				throw new ArgumentException("Invalid day of the week");
		}

		Type = type;
		Height = double.Parse(height);
		Date = DateOnly.FromDateTime(DateTime.Parse(date));
		Timeframe = TimeOnly.FromDateTime(DateTime.Parse(timeframe));
	}

	public override string ToString()
	{
		return $"{Day} {Type} tide at {Timeframe.ToShortTimeString()} of {Height} feet.";
	}

	public string WithinRangeTideString(Village village)
	{
		return $"The tide tables for {village.ToShortString()} show a {Type} tide of {Height} feet at {Timeframe}.";
	}
	public string OutOfRangeTideString(Village village)
	{
		return $"The next {Type} tide at {village.ToShortString()} will be at {Timeframe.ToShortTimeString()} of {Height} feet.";
	}

	public string RegularTideInfoString()
	{
		return $"At {Timeframe}, {Type} tide of {Height} feet.";
	}
}
