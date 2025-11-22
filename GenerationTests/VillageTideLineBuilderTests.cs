using GeneratoreCore.Builders;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

namespace GenerationTests;

public class VillageTideLineBuilderTests
{
	private static TideInfo MakeTide(string time, string type = "High", string height = "12.3")
	{
		// Use a fixed Monday date matching day code "Mon" for stability
		return new TideInfo("2025-01-06", "Mon", type, height, time);
	}

	private static TimeWindow MakeWindow(string start, string end)
	{
		return new TimeWindow(TimeOnly.FromDateTime(DateTime.Parse(start)), TimeOnly.FromDateTime(DateTime.Parse(end)));
	}

	[Fact]
	public void BuildPrimary_NoTides_ReturnsNoDataMessage()
	{
		var window = MakeWindow("9:00 AM", "1:00 PM");
		var line = VillageTideLineBuilder.BuildPrimary(new List<TideInfo>(), Village.Bethel, window);
		Assert.Equal("No tide data available for Bethel.", line);
	}

	[Fact]
	public void BuildPrimary_WithinWindow_UsesWithinRangeFormat()
	{
		var tide = MakeTide("10:15 AM");
		var window = MakeWindow("9:00 AM", "1:00 PM");
		var line = VillageTideLineBuilder.BuildPrimary(new List<TideInfo> { tide }, Village.Bethel, window);
		Assert.StartsWith("The tide tables for Bethel show a", line);
		Assert.Contains("High tide", line);
		Assert.Contains("10:15", line); // TimeOnly default formatting contains HH:mm
		Assert.Contains("12.3 feet", line);
	}

	[Fact]
	public void BuildPrimary_OutOfWindow_UsesOutOfRangeFormat()
	{
		var tide = MakeTide("3:30 PM");
		var window = MakeWindow("9:00 AM", "2:00 PM"); // End before tide
		var line = VillageTideLineBuilder.BuildPrimary(new List<TideInfo> { tide }, Village.Bethel, window);
		Assert.StartsWith("The next High tide at Bethel will be", line);
		Assert.Contains("3:30", line); // Short time string
		Assert.Contains("12.3 feet", line);
	}

	[Fact]
	public void BuildSecondary_NoSecondTide_ReturnsPlaceholder()
	{
		var tide = MakeTide("10:15 AM");
		var line = VillageTideLineBuilder.BuildSecondary(new List<TideInfo> { tide });
		Assert.Equal("-- No other tides --", line);
	}

	[Fact]
	public void BuildSecondary_WithSecondTide_ReturnsRegularInfo()
	{
		var tides = new List<TideInfo> { MakeTide("10:15 AM"), MakeTide("1:45 PM", "Low", "3.4") };
		var line = VillageTideLineBuilder.BuildSecondary(tides);
		Assert.StartsWith("At", line);
		Assert.Contains("Low tide", line);
		Assert.Contains("3.4 feet", line);
		Assert.True(line.Contains("13:45") || line.Contains("1:45"), $"Expected time fragment not found in line: {line}");
	}

	[Fact]
	public void BuildTertiary_NoThirdTide_ReturnsEmpty()
	{
		var tides = new List<TideInfo> { MakeTide("10:15 AM"), MakeTide("1:45 PM") };
		var line = VillageTideLineBuilder.BuildTertiary(tides);
		Assert.Equal(string.Empty, line);
	}

	[Fact]
	public void BuildTertiary_WithThirdTide_ReturnsRegularInfo()
	{
		var tides = new List<TideInfo> { MakeTide("10:15 AM"), MakeTide("1:45 PM"), MakeTide("6:05 PM", "Low", "2.8") };
		var line = VillageTideLineBuilder.BuildTertiary(tides);
		Assert.StartsWith("At", line);
		Assert.Contains("Low tide", line);
		Assert.Contains("2.8 feet", line);
	}

	[Fact]
	public void AggregateLines_FiltersEmptyTertiary()
	{
		var tides = new List<TideInfo> { MakeTide("10:15 AM"), MakeTide("1:45 PM") };
		var window = MakeWindow("9:00 AM", "2:00 PM");
		var agg = VillageTideLineBuilder.AggregateLines(tides, Village.Bethel, window);
		var lines = agg.Split('\n');
		Assert.Equal(2, lines.Length); // primary + secondary only
		Assert.DoesNotContain(lines, l => string.IsNullOrWhiteSpace(l));
	}

	[Fact]
	public void AggregateLines_IncludesAllThree()
	{
		var tides = new List<TideInfo> { MakeTide("10:15 AM"), MakeTide("1:45 PM"), MakeTide("6:05 PM") };
		var window = MakeWindow("9:00 AM", "11:00 AM"); // third tide still within/outside window not relevant for inclusion
		var agg = VillageTideLineBuilder.AggregateLines(tides, Village.Bethel, window);
		var lines = agg.Split('\n');
		Assert.Equal(3, lines.Length);
	}

	[Fact]
	public void RegularTideInfoString_Format()
	{
		var tide = MakeTide("10:15 AM", "Low", "3.4");
		var line = VillageTideLineBuilder.RegularTideInfoString(tide);
		Assert.StartsWith("At", line);
		Assert.Contains("Low tide", line);
		Assert.Contains("3.4 feet", line);
	}

	[Fact]
	public void OutOfRangeTideString_Format()
	{
		var tide = MakeTide("3:30 PM", "High", "12.3");
		var line = VillageTideLineBuilder.OutOfRangeTideString(tide, Village.Quinhagak);
		Assert.StartsWith("The next High tide at Quinhagak will be", line);
		Assert.Contains("3:30", line);
		Assert.Contains("12.3 feet", line);
	}

	[Fact]
	public void WithinRangeTideString_Format()
	{
		var tide = MakeTide("10:15 AM", "High", "12.3");
		var line = VillageTideLineBuilder.WithinRangeTideString(tide, Village.Togiak);
		Assert.StartsWith("The tide tables for Togiak at Black Rock show a", line);
		Assert.Contains("High tide", line);
		Assert.Contains("12.3 feet", line);
	}
}
