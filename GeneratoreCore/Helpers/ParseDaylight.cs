using GeneratorCore.Models;
using IronXL;

namespace GeneratorCore.Helpers;

class ParseDaylight
{
	private static ParseDaylight? instance = null;

	public static ParseDaylight GetInstance()
	{
		if (instance == null)
		{
			instance = new ParseDaylight();
		}

		return instance;
	}

	private ParseDaylight()
	{
		LoadTideInfo();
	}

	List<DaylightRecord> allDaylightRecords = new List<DaylightRecord>();

	private void LoadTideInfo()
	{
		Console.WriteLine("Loading Daylight record info...");
		string filePath = Path.Combine("Files", "2025 Bethel Daylight.xlsx");

		// Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
		WorkBook workbook = WorkBook.Load(filePath);
		WorkSheet sheet = workbook.WorkSheets.First();

		for (int i = 2; i < 366; i++)
		{
			// Get all cells for the daylight record
			var cells = sheet[$"A{i}:I{i}"].ToList();

			// Skip empty rows
			if (cells[0].IsEmpty || cells[0].Text.Contains('=')) continue;

			// Parse the daylight record
			string month = cells[1].Text;
			string day = cells[2].StringValue;
			string sunrise = cells[3].StringValue;
			string sunset = cells[4].StringValue;
			string duration = cells[5].Text;
			string gain = cells[6].Text;
			string loss = cells[7].Text;
			string notes = cells[8].Text;

			DaylightRecord tide = new DaylightRecord(month, day, sunrise, sunset, duration, gain, loss, notes);
			allDaylightRecords.Add(tide);
		}
	}

	public DaylightRecord GetDaylightInfoForDay(int month, int day)
	{
		return allDaylightRecords.First(x => x.Date.Month == month && x.Date.Day == day);
	}
}
