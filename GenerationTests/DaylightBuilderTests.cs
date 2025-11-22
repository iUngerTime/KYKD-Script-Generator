using GeneratorCore.Models;
using GeneratorCore.Models.Enums;
using GeneratoreCore.Builders;

namespace GenerationTests;

public class DaylightBuilderTests
{
	private DaylightRecord MakeRecord(string sunrise, string sunset, string duration)
	{
		// Month/day arbitrary (must parse); choose Monday Jan 6 2025 for consistent window.
		return new DaylightRecord("January", "06", sunrise, sunset, duration, "", "more");
	}

	[Fact]
	public void Sunrise_Morning_UsesPastTense()
	{
		DaylightRecord r = MakeRecord("5:55 AM", "5:30 PM", "08:00");
		string line = DaylightLineBuilder.BuildSunrise(r, TimeOfDay.Morning);
		Assert.Contains("The sun rose this morning", line);
		Assert.Contains("5:55 am", line);
	}

	[Fact]
	public void Sunrise_Morning_PresentTense()
	{
		DaylightRecord r = MakeRecord("8:30 AM", "5:30 PM", "09:00");
		string line = DaylightLineBuilder.BuildSunrise(r, TimeOfDay.Morning);
		Assert.StartsWith("Sunrise this morning", line);
	}

	[Fact]
	public void Sunrise_Morning_FutureTense()
	{
		DaylightRecord r = MakeRecord("11:30 AM", "5:30 PM", "09:00");
		string line = DaylightLineBuilder.BuildSunrise(r, TimeOfDay.Morning);
		Assert.StartsWith("The sun will rise later", line);
	}

	[Fact]
	public void Sunrise_Afternoon_PastTense()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "5:30 PM", "09:00");
		string line = DaylightLineBuilder.BuildSunrise(r, TimeOfDay.Afternoon);
		Assert.StartsWith("The sun rose this morning", line);
	}

	[Fact]
	public void Sunset_BeforeWindow_PastTense()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "5:30 PM", "10:00");
		string line = DaylightLineBuilder.BuildSunset(r, TimeOfDay.Evening);
		Assert.Contains("sun set in Bethel this afternoon/evening", line);
		Assert.Contains("5:30 pm", line);
	}

	[Fact]
	public void Sunset_AfterWindow_FutureTense()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "11:30 PM", "16:00");
		string line = DaylightLineBuilder.BuildSunset(r, TimeOfDay.Evening);
		Assert.Contains("will set this evening", line);
		Assert.Contains("11:30 pm", line);
	}

	[Fact]
	public void Sunset_AfterMidnight_MinutesAfterMidnight()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "12:25 AM", "16:55");
		string line = DaylightLineBuilder.BuildSunset(r, TimeOfDay.Evening);
		Assert.Contains("minutes after midnight", line);
		Assert.Contains("25 minutes", line);
	}

	[Fact]
	public void DaylightLength_BeforeWindow_PastTense()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "5:30 PM", "10:00");
		string line = DaylightLineBuilder.BuildDaylightLength(r, TimeOfDay.Evening);
		Assert.StartsWith("That gave us", line);
	}

	[Fact]
	public void DaylightLength_InWindow_PresentParticiple()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "7:08 PM", "11:00");
		string line = DaylightLineBuilder.BuildDaylightLength(r, TimeOfDay.Evening);
		Assert.StartsWith("Giving us", line);
	}

	[Fact]
	public void DaylightLength_AfterWindow_FutureTense()
	{
		DaylightRecord r = MakeRecord("7:30 AM", "11:30 PM", "16:00");
		string line = DaylightLineBuilder.BuildDaylightLength(r, TimeOfDay.Evening);
		Assert.StartsWith("That will give us", line);
	}
}
