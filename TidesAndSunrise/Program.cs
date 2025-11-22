using TidesAndSunrise.Enums;
using TidesAndSunrisesScriptGenerator.Helpers;


//List<TideInfo> results = parseTides.GetTideDetailsForDay(Village.Togiak, TimeOfDay.Afternoon, 6, 6);
//foreach (var tide in results)
//{
//    Console.WriteLine(tide.ToString());
//}

int year = 2026;
for (int month = 3; month <= 12; month++)
{
    int daysInMonth = DateTime.DaysInMonth(year, month);
    for (int day = 1; day <= daysInMonth; day++)
    {
        ScriptGenerator.CreateTideScript(month, day, TimeOfDay.Morning);
        ScriptGenerator.CreateTideScript(month, day, TimeOfDay.Evening);
    }
}

//scriptGenerator.CreateTideScript(3, 9, TimeOfDay.Evening);