namespace GeneratoreCore;

public sealed class ScriptContext
{
	public string PeriodLabel { get; init; } = string.Empty;

	public string DayOfWeek { get; init; } = string.Empty;

	public string Month { get; init; } = string.Empty;

	public int Day { get; init; }

	public string SunriseLine { get; init; } = string.Empty;

	public string SunsetLine { get; init; } = string.Empty;

	public string DaylightLine { get; init; } = string.Empty;

	public string BethelTideLine { get; init; } = string.Empty;

	public string QuinhagakTideLine { get; init; } = string.Empty;

	public string TogiakTideLine { get; init; } = string.Empty;

	public Dictionary<string, string> AsDictionary() => new()
	{
		["PeriodLabel"] = PeriodLabel,
		["DayOfWeek"] = DayOfWeek,
		["Month"] = Month,
		["Day"] = Day.ToString(),
		["SunriseLine"] = SunriseLine,
		["SunsetLine"] = SunsetLine,
		["DaylightLine"] = DaylightLine,
		["BethelTideLine"] = BethelTideLine,
		["QuinhagakTideLine"] = QuinhagakTideLine,
		["TogiakTideLine"] = TogiakTideLine
	};
}
