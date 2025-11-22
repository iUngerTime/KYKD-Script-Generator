using GeneratorCore.Models.Enums;
using GeneratoreCore;

// Configuration
int year = 2026;
string resourceDir = $"{year} Tides and Daylight Scripts";
string tidesExcel = $"{year} Tides.xlsx";
string daylightExcel = $"{year} Bethel Daylight.xlsx";
string scriptTemplate = "script-template.txt";
string outputDir = "Scripts";
TimeOfDay[] periods = [TimeOfDay.Morning, TimeOfDay.Evening];

// Create all relevant resource paths
string baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Resources", resourceDir));
string daylightExcelPath = Path.Combine(baseDir, daylightExcel);
string tidesExcelPath = Path.Combine(baseDir, tidesExcel);
string templatePath = Path.Combine(baseDir, scriptTemplate);
string scriptsOutputDir = Path.Combine(baseDir, outputDir);

Directory.CreateDirectory(scriptsOutputDir);

DefaultScriptDataProvider provider = new();
TideScriptGenerator generator = new(provider, daylightExcelPath, tidesExcelPath, templatePath);

// Loop through all days of the year
for (int month = 1; month <= 12; month++)
{
	int daysInMonth = DateTime.DaysInMonth(year, month);
	for (int day = 1; day <= daysInMonth; day++)
	{
		DateOnly date = new(year, month, day);
		foreach (TimeOfDay period in periods)
		{
			string content = generator.GenerateScriptForDay(date, period);
			string fileName = $"{month}-{day}-Tides-{period.ToShortString()}-GeneratedScript.txt";
			string filePath = Path.Combine(scriptsOutputDir, fileName);

			File.WriteAllText(filePath, content);

			Console.WriteLine($"Wrote {fileName}");
		}
	}
}

Console.WriteLine("Script generation complete.");