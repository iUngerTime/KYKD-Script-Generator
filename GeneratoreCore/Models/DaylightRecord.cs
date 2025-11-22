namespace GeneratorCore.Models;

public class DaylightRecord
{
	public DateOnly Date { get; set; }

	public TimeOnly Sunrise { get; set; }

	public TimeOnly Sunset { get; set; }

	public TimeOnly Duration { get; set; }

	public TimeOnly? Change { get; set; }

	/// <summary>
	/// Going to be "more" or "less"
	/// </summary>
	public string ChangeQuantifier { get; set; }

	public DaylightRecord(string month, string day, string sunrise, string sunset, string duration, string change, string changeQuantifier)
	{
		Date = DateOnly.Parse($"{month} {day}");
		Sunrise = TimeOnly.FromDateTime(DateTime.Parse(sunrise));
		Sunset = TimeOnly.FromDateTime(DateTime.Parse(sunset));
		Duration = TimeOnly.FromDateTime(DateTime.Parse(duration));
		Change = string.IsNullOrWhiteSpace(change) ? null : TimeOnly.FromDateTime(DateTime.Parse(change));
		ChangeQuantifier = changeQuantifier;
	}

	public override string ToString()
	{
		return $"{Date:MMMM dd}, Sunrise: {Sunrise}, Sunset: {Sunset}, Duration: {Duration}, Change: {Change}, Change Quantifier: {ChangeQuantifier}";
	}
}
