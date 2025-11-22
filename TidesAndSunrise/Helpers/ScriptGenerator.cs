using System.Text;
using TidesAndSunrise.Enums;
using TidesAndSunrise.Helpers;
using TidesAndSunrise.Models;

namespace TidesAndSunrisesScriptGenerator.Helpers;

public static class ScriptGenerator
{
	public static StringBuilder CreateTideScript(int month, int day, TimeOfDay timeOfDay)
	{
		DateOnly date = new DateOnly(2025, month, day);
		TimeWindow timeWindow = TimeWindowCalculator.GetTimeWindowForDate(month, day, timeOfDay);
		DaylightRecord? daylightInfo = ParseDaylight.GetInstance().GetDaylightInfoForDay(month, day);

		var script = new StringBuilder();

		WriteIntro(script, timeOfDay, date);

		script.AppendLine();
		script.AppendLine(daylightInfo.SunriseString(timeOfDay, timeWindow));
		script.AppendLine(daylightInfo.SunsetString(timeOfDay, timeWindow));
		script.AppendLine(daylightInfo.TimeDifferenceString());

		foreach (var village in Enum.GetValues<Village>())
		{
			var tidesForVillage = ParseTides.GetInstance().GetTideDetailsForDay(village, timeOfDay, month, day);
			WriteVillageTides(script, timeOfDay, village, timeWindow, tidesForVillage);
		}

		WriteOutro(script);

		return script;
	}

	public static void WriteTideScriptToFileSystem(int month, int day, TimeOfDay timeOfDay, StringBuilder script)
	{
		// Output file path
		string fileName = $"{month}-{day}-Tides-{timeOfDay.ToShortString()}-GeneratedScript.txt";
		string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		string filePath = Path.Combine(desktopPath, "Scripts", fileName);

		// Write to file
		Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
		File.WriteAllText(filePath, script.ToString());

		Console.WriteLine($"Tide schedule written to {fileName}");
	}

	private static void WriteIntro(StringBuilder writer, TimeOfDay timeOfDay, DateOnly date)
	{
		writer.AppendLine($"{timeOfDay.GreetingString()},\nNow let's look at the KYKD sunrise and sunset times, and the tide predictions for today, {date.DayOfWeek}, {date.ToString("MMMM d")}");
	}

	private static void WriteVillageTides(StringBuilder writer, TimeOfDay timeOfDay, Village village, TimeWindow timeWindow, List<TideInfo> tides)
	{
		if (tides.Count == 0)
		{
			Console.Error.WriteLine("No tides found for this village.");
			return;
		}

		writer.AppendLine();

		// Determine if it was within the range or not
		bool isPastRange = tides[0].Timeframe > timeWindow.End;

		// Get the tides for the village
		if (isPastRange)
		{
			writer.AppendLine(tides[0].OutOfRangeTideString(village));
		}
		else
		{
			writer.AppendLine(tides[0].WithinRangeTideString(village));
		}

		if (tides.Count > 1)
		{
			writer.AppendLine(tides[1].RegularTideInfoString());
		}
		else
		{
			writer.AppendLine("There is no secondary high or low tide.");
			Console.Error.WriteLine("It's missing a second tide");
		}

		if (tides.Count > 2)
		{
			writer.AppendLine(tides[2].RegularTideInfoString());
		}
		else
		{
			Console.Error.WriteLine("It's missing a third tide");
		}
	}

	private static void WriteOutro(StringBuilder writer)
	{
		writer.AppendLine("\n\nI'm _________ and you are tuned to KYKD Bethel, giving the winds a mighty voice.");
	}
}
