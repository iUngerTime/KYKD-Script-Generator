using GeneratorCore.Models;
using GeneratorCore.Helpers;

namespace GeneratoreCore.Parsers;

public class DaylightParser
{
	private static DaylightParser? instance = null;

	public static DaylightParser GetInstance()
	{
		if (instance == null)
		{
			instance = new DaylightParser();
		}

		return instance;
	}

	private DaylightParser()
	{ }

	List<DaylightRecord> allDaylightRecords = new();

	public void LoadDaylightRecords(string excelDataPath)
	{
		List<object?[]> rows = ExcelSheetReader.ReadFirstSheet(excelDataPath);

		// Row 1 is the header; data runs until the rows go blank (365 or 366 days).
		foreach (object?[] row in rows.Skip(1))
		{
			// Skip empty rows
			if (ExcelSheetReader.IsSkippable(ExcelSheetReader.Cell(row, 0))) continue;

			// Parse the daylight record
			string month = ExcelSheetReader.ToInvariantString(ExcelSheetReader.Cell(row, 1));
			string day = ExcelSheetReader.ToInvariantString(ExcelSheetReader.Cell(row, 2));
			string sunrise = ExcelSheetReader.ToTimeString(ExcelSheetReader.Cell(row, 3));
			string sunset = ExcelSheetReader.ToTimeString(ExcelSheetReader.Cell(row, 4));
			string duration = ExcelSheetReader.ToTimeString(ExcelSheetReader.Cell(row, 5));
			string change = ExcelSheetReader.ToTimeString(ExcelSheetReader.Cell(row, 6));
			string quantifier = ExcelSheetReader.ToInvariantString(ExcelSheetReader.Cell(row, 7));

			DaylightRecord tide = new(month, day, sunrise, sunset, duration, change, quantifier);
			allDaylightRecords.Add(tide);
		}
	}

	public DaylightRecord GetDaylightInfoForDay(int month, int day)
	{
		return allDaylightRecords.First(x => x.Date.Month == month && x.Date.Day == day);
	}
}
