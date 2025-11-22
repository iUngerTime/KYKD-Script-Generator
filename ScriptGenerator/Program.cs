using GeneratorCore.Models.Enums;
using GeneratoreCore;

// Configuration
int year = 2026;
string resourceDir = $"{year} Tides and Daylight Scripts";
TimeOfDay[] periods = [TimeOfDay.Morning, TimeOfDay.Evening];

// Resolve template path relative to build output
string templatePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Resources", resourceDir, "script-template.txt"));
string scriptsDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Resources", resourceDir, "Scripts"));
Directory.CreateDirectory(scriptsDir);

var provider = new DefaultScriptDataProvider();
var generator = new TideScriptGenerator(provider, templatePath);

// Loop through all days of the year
for (int month = 1; month <= 12; month++)
{
	int daysInMonth = DateTime.DaysInMonth(year, month);
	for (int day = 1; day <= daysInMonth; day++)
	{
		var date = new DateOnly(year, month, day);
		foreach (var period in periods)
		{
			string content = generator.Generate(date, period);
			string fileName = $"{month}-{day}-Tides-{period.ToShortString()}-GeneratedScript.txt";
			string filePath = Path.Combine(scriptsDir, fileName);

			File.WriteAllText(filePath, content);

			Console.WriteLine($"Wrote {fileName}");
		}
	}
}

Console.WriteLine("Script generation complete.");