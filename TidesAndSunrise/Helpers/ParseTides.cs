using IronXL;
using TidesAndSunrise.Enums;
using TidesAndSunrise.Models;
using TidesAndSunrisesScriptGenerator.Helpers;

namespace TidesAndSunrise.Helpers;

public class ParseTides
{
    private static ParseTides? instance;
    public static ParseTides GetInstance()
    {
        if (instance == null)
        {
            instance = new ParseTides();
        }
        return instance;
    }

    private ParseTides()
    {
        LoadTideInfo();
    }

    List<TideDay> allTidesAllVilages = new List<TideDay>();

    private void LoadTideInfo()
    {
        Console.WriteLine("Loading tide info...\n\n");
        string filePath = Path.Combine("Files", "2025 Tides.xlsx");

        // Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
        WorkBook workbook = WorkBook.Load(filePath);
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
        foreach (var tide in allTidesAllVilages)
        {
            Console.WriteLine(tide.ToString());
        }
    }

    public List<TideInfo> GetTideDetailsForDay(Village village, TimeOfDay timeOfDay, int month, int day)
    {
        var yearlyTides = GetVillageTidesForYear(village);

        var dayTides = yearlyTides.First(t => t.Date.Month == month && t.Date.Day == day);

        // Compute the next date from dayTides.Date and get those tides
        DateOnly nextDate = dayTides.Date.AddDays(1);
        var nextDayTides = yearlyTides.FirstOrDefault(t => t.Date == nextDate);

        // Work with the tide infos
        List<TideInfo> tideInfos = dayTides.Tides;

        // Get the window of time
        TimeWindow timeWindow = TimeWindowCalculator.GetTimeWindowForDate(month, day, timeOfDay);

        // Get tides that occur after window start (sorted by time)
        var tidesAfterWindow = dayTides.Tides
            .Where(t => t.Timeframe >= timeWindow.Start)
            .OrderBy(t => t.Timeframe)
            .ToList();

        // Collect up to 3 tides
        List<TideInfo> finalTides = tidesAfterWindow.Take(3).ToList();

        // If fewer than 3 found, pull remainder from next day
        if (finalTides.Count < 3 && nextDayTides != null)
        {
            int needed = 3 - finalTides.Count;
            var nextDayTidesList = nextDayTides.Tides
                .OrderBy(t => t.Timeframe)
                .ToList();
            finalTides.AddRange(nextDayTidesList.Take(needed));
        }

        return finalTides;
    }

    private List<TideDay> ParseTideGroup(WorkSheet sheet, string startCol, string endCol, Village village)
    {
        List<TideInfo> tides = new List<TideInfo>();

        // Loop through the rows
        // Start at 4 because it's the first row of data
        for (int i = 4; i < 1825; i++)
        {
            // Get all cells for the day
            var cells = sheet[$"{startCol}{i}:{endCol}{i}"].ToList();

            // Skip empty rows
            if (cells[0].IsEmpty || cells[0].Text.Contains('=')) continue;

            // The first cell is the day of the week
            string date = cells[0].Text;
            string day = cells[1].Text;
            string type = cells[2].Text;
            string height = cells[3].Text;
            string time = cells[4].Text;

            TideInfo tide = new TideInfo(date, day, type, height, time);
            tides.Add(tide);
        }

        // Group by date
        var groupedTides = tides.GroupBy(t => t.Date).ToList();
        List<TideDay> tideDays = new List<TideDay>();
        foreach (var group in groupedTides)
        {
            TideDay tideDay = new TideDay(group.Key, village);
            foreach (var tide in group)
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
        if (allTidesAllVilages == null)
        {
            LoadTideInfo();
        }

        return allTidesAllVilages!.Where(x => x.RelatedVillage == village).ToList();
    }
}
