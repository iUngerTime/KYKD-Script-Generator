using GeneratoreCore.Parsers;

namespace GenerationTests;

public class ParsingTests
{
	[Fact]
	public void Parse2025Data()
	{
		// Configuration
		int year = 2025;
		string resourceDir = $"{year} Tides and Daylight Scripts";
		string tidesExcel = $"{year} Tides.xlsx";
		string daylightExcel = $"{year} Bethel Daylight.xlsx";

		// Create all relevant resource paths
		string baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Resources", resourceDir));
		string daylightExcelPath = Path.Combine(baseDir, daylightExcel);
		string tidesExcelPath = Path.Combine(baseDir, tidesExcel);

		var ex = Record.Exception(() =>
		{
			DaylightParser.GetInstance().LoadDaylightRecords(daylightExcelPath);
			TideParser.GetInstance().LoadTideInfo(tidesExcelPath);
		});
		Assert.Null(ex);
	}

	[Fact]
	public void Parse2026Data()
	{
		// Configuration
		int year = 2026;
		string resourceDir = $"{year} Tides and Daylight Scripts";
		string tidesExcel = $"{year} Tides.xlsx";
		string daylightExcel = $"{year} Bethel Daylight.xlsx";

		// Create all relevant resource paths
		string baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Resources", resourceDir));
		string daylightExcelPath = Path.Combine(baseDir, daylightExcel);
		string tidesExcelPath = Path.Combine(baseDir, tidesExcel);

		var ex = Record.Exception(() =>
		{
			DaylightParser.GetInstance().LoadDaylightRecords(daylightExcelPath);
			TideParser.GetInstance().LoadTideInfo(tidesExcelPath);
		});
		Assert.Null(ex);
	}
}
