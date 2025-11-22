using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;
using IronXL;

namespace GeneratoreCore.Parsers;

public class TideParser
{
	private static TideParser? instance;

	public static TideParser GetInstance()
	{
		if (instance == null)
		{
			instance = new TideParser();
		}

		return instance;
	}

	private TideParser()
	{ }

	List<TideDay> allTidesAllVilages = new();

	public void LoadTideInfo(string excelDataPath)
	{
		// Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
		WorkBook workbook = WorkBook.Load(excelDataPath);
		WorkSheet sheet = workbook.WorkSheets.First();

		List<TideDay> tides =
		[
			.. ParseTideGroup(sheet, "B", "F", Village.Bethel),
			.. ParseTideGroup(sheet, "H", "L", Village.Quinhagak),
			.. ParseTideGroup(sheet, "N", "R", Village.Togiak),
		];

		allTidesAllVilages = tides;
	}

	public void ParseAllTides()
	{
		foreach (TideDay tide in allTidesAllVilages)
		{
			Console.WriteLine(tide.ToString());
		}
	}

	public List<TideInfo> GetTideDetailsForDate(Village village, DateOnly date, TimeOfDay timeOfDay)
	{
		List<TideDay> yearlyTides = GetVillageTidesForYear(village);

		TideDay dayTides = yearlyTides.First(t => t.Date.Month == date.Month && t.Date.Day == date.Day);

		// Compute the next date from dayTides.Date and get those tides
		DateOnly nextDate = dayTides.Date.AddDays(1);
		TideDay? nextDayTides = yearlyTides.FirstOrDefault(t => t.Date == nextDate);

		// Work with the tide infos
		List<TideInfo> tideInfos = dayTides.Tides;

		// Get the window of time
		TimeWindow timeWindow = TimeWindowCalculator.GetTimeWindow(date, timeOfDay);

		// Get tides that occur after window start (sorted by time)
		List<TideInfo> tidesAfterWindow = dayTides.Tides
			.Where(t => t.Timeframe >= timeWindow.Start)
			.OrderBy(t => t.Timeframe)
			.ToList();

		// Collect up to 3 tides
		List<TideInfo> finalTides = tidesAfterWindow.Take(3).ToList();

		// If fewer than 3 found, pull remainder from next day
		if (finalTides.Count < 3 && nextDayTides != null)
		{
			int needed = 3 - finalTides.Count;
			List<TideInfo> nextDayTidesList = nextDayTides.Tides
				.OrderBy(t => t.Timeframe)
				.ToList();
			finalTides.AddRange(nextDayTidesList.Take(needed));
		}

		return finalTides;
	}

	private List<TideDay> ParseTideGroup(WorkSheet sheet, string startCol, string endCol, Village village)
	{
		List<TideInfo> tides = new();

		// Loop through the rows
		// Start at 4 because it's the first row of data
		for (int i = 4; i < 1825; i++)
		{
			// Get all cells for the day
			List<Cell> cells = sheet[$"{startCol}{i}:{endCol}{i}"].ToList();

			// Skip empty rows
			if (cells[0].IsEmpty || cells[0].Text.Contains('=')) continue;

			// The first cell is the day of the week
			string date = cells[0].Text;
			string day = cells[1].Text;
			string type = cells[2].Text;
			string height = cells[3].Text;
			string time = cells[4].Text;

			TideInfo tide = new(date, day, type, height, time);
			tides.Add(tide);
		}

		// Group by date
		List<IGrouping<DateOnly, TideInfo>> groupedTides = tides.GroupBy(t => t.Date).ToList();
		List<TideDay> tideDays = new();
		foreach (IGrouping<DateOnly, TideInfo>? group in groupedTides)
		{
			TideDay tideDay = new(group.Key, village);
			foreach (TideInfo? tide in group)
			{
				tideDay.AddTide(tide);
			}
			tideDays.Add(tideDay);
		}

		// Sort by date
		tideDays = tideDays.OrderBy(t => t.Date).ToList();
		return tideDays;
	}

	private List<TideDay> GetVillageTidesForYear(Village village)
	{
		return allTidesAllVilages!.Where(x => x.RelatedVillage == village).ToList();
	}
}
