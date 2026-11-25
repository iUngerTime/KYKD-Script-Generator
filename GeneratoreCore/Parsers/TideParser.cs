using GeneratorCore.Helpers;
using GeneratorCore.Models;
using GeneratorCore.Models.Enums;

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

	/// <summary>
	/// Loads every village's tides from the tides spreadsheet.
	/// </summary>
	/// <param name="excelDataPath">Path to the tides xlsx file.</param>
	/// <param name="year">The year the sheet starts in. The sheet's date cells only display day and month,
	/// and their stored year is often stale, so it is used only when no year is given.</param>
	public void LoadTideInfo(string excelDataPath, int? year = null)
	{
		List<object?[]> rows = ExcelSheetReader.ReadFirstSheet(excelDataPath);

		List<TideDay> tides =
		[
			.. ParseTideGroup(rows, 1, Village.Bethel, year),
			.. ParseTideGroup(rows, 7, Village.Quinhagak, year),
			.. ParseTideGroup(rows, 13, Village.Togiak, year),
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

	/// <summary>
	/// Parses one village's five-column block (Date, Day, Type, Height, Time).
	/// </summary>
	/// <param name="rows">All sheet rows.</param>
	/// <param name="startColumn">Zero-based index of the block's Date column.</param>
	/// <param name="village">The village the block belongs to.</param>
	/// <param name="year">The year the sheet starts in, or null to use the first date cell's year.</param>
	/// <returns>The village's tides grouped by day, in date order.</returns>
	private List<TideDay> ParseTideGroup(List<object?[]> rows, int startColumn, Village village, int? year)
	{
		List<TideInfo> tides = new();
		int? currentYear = year;
		int previousMonth = 0;

		// Rows 1-3 are the village name, the column headers, and a "====" separator.
		foreach (object?[] row in rows.Skip(3))
		{
			object? dateCell = ExcelSheetReader.Cell(row, startColumn);

			// Skip empty rows
			if (ExcelSheetReader.IsSkippable(dateCell)) continue;

			DateTime cellDate = Convert.ToDateTime(dateCell, System.Globalization.CultureInfo.InvariantCulture);
			currentYear ??= cellDate.Year;

			// The sheet runs into the next January (Dec 31 evening needs Jan 1 tides).
			if (cellDate.Month < previousMonth) currentYear++;
			previousMonth = cellDate.Month;

			string date = new DateOnly(currentYear.Value, cellDate.Month, cellDate.Day).ToString("yyyy-MM-dd");
			string day = ExcelSheetReader.ToInvariantString(ExcelSheetReader.Cell(row, startColumn + 1));
			string type = ExcelSheetReader.ToInvariantString(ExcelSheetReader.Cell(row, startColumn + 2));
			string height = ExcelSheetReader.ToInvariantString(ExcelSheetReader.Cell(row, startColumn + 3));
			string time = ExcelSheetReader.ToTimeString(ExcelSheetReader.Cell(row, startColumn + 4));

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
