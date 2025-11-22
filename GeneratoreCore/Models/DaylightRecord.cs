namespace GeneratorCore.Models;

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
}
