using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

namespace GeneratoreCore.Builders;

public static class VillageTideLineBuilder
{
	public static string BuildPrimary(List<TideInfo> tides, Village village, TimeWindow window)
	{
		if (tides.Count == 0) return $"No tide data available for {village}.";
		bool isPastRange = tides[0].Timeframe > window.End;
		return isPastRange ? tides[0].OutOfRangeTideString(village) : tides[0].WithinRangeTideString(village);
	}

	public static string BuildSecondary(List<TideInfo> tides)
		 => tides.Count > 1 ? tides[1].RegularTideInfoString() : "There is no secondary high or low tide.";

	public static string BuildTertiary(List<TideInfo> tides)
		 => tides.Count > 2 ? tides[2].RegularTideInfoString() : string.Empty;

	public static string AggregateLines(List<TideInfo> tides, Village village, TimeWindow window)
	{
		var primary = BuildPrimary(tides, village, window);
		var secondary = BuildSecondary(tides);
		var tertiary = BuildTertiary(tides);
		return string.Join('\n', new[] { primary, secondary, tertiary }.Where(s => !string.IsNullOrWhiteSpace(s)));
	}
}
