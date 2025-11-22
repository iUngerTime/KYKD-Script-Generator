using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

namespace GeneratoreCore.Builders;

public static class VillageTideLineBuilder
{
	public static string BuildPrimary(List<TideInfo> tides, Village village, TimeWindow window)
	{
		if (tides.Count == 0)
		{
			return $"No tide data available for {village}.";
		}

		bool isPastRange = tides[0].Timeframe > window.End;
		return isPastRange ? OutOfRangeTideString(tides[0], village) : WithinRangeTideString(tides[0], village);
	}

	public static string BuildSecondary(List<TideInfo> tides)
	{
		return tides.Count > 1 ? RegularTideInfoString(tides[1]) : "-- No other tides --";
	}

	public static string BuildTertiary(List<TideInfo> tides)
	{
		return tides.Count > 2 ? RegularTideInfoString(tides[2]) : string.Empty;
	}

	public static string AggregateLines(List<TideInfo> tides, Village village, TimeWindow window)
	{
		string primary = BuildPrimary(tides, village, window);
		string secondary = BuildSecondary(tides);
		string tertiary = BuildTertiary(tides);
		return string.Join('\n', new[] { primary, secondary, tertiary }.Where(s => !string.IsNullOrWhiteSpace(s)));
	}

	public static string WithinRangeTideString(TideInfo tide, Village village)
	{
		return $"The tide tables for {village.ToShortString()} show a {tide.Type} tide of {tide.Height} feet at {tide.Timeframe}.";
	}

	public static string OutOfRangeTideString(TideInfo tide, Village village)
	{
		return $"The next {tide.Type} tide at {village.ToShortString()} will be at {tide.Timeframe.ToShortTimeString()} of {tide.Height} feet.";
	}

	public static string RegularTideInfoString(TideInfo tide)
	{
		return $"At {tide.Timeframe}, {tide.Type} tide of {tide.Height} feet.";
	}
}
