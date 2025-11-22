using GeneratorCore.Models;
using IronXL;

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
		// Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
		WorkBook workbook = WorkBook.Load(excelDataPath);
		WorkSheet sheet = workbook.WorkSheets.First();

		for (int i = 2; i < 366; i++)
		{
			// Get all cells for the daylight record
			List<Cell> cells = sheet[$"A{i}:I{i}"].ToList();

			// Skip empty rows
			if (cells[0].IsEmpty || cells[0].Text.Contains('=')) continue;

			// Parse the daylight record
			string month = cells[1].Text;
			string day = cells[2].StringValue;
			string sunrise = cells[3].StringValue;
			string sunset = cells[4].StringValue;
			string duration = cells[5].Text;
			string change = cells[6].Text;
			string quantifier = cells[7].Text;

			DaylightRecord tide = new(month, day, sunrise, sunset, duration, change, quantifier);
			allDaylightRecords.Add(tide);
		}
	}

	public DaylightRecord GetDaylightInfoForDay(int month, int day)
	{
		return allDaylightRecords.First(x => x.Date.Month == month && x.Date.Day == day);
	}
}
