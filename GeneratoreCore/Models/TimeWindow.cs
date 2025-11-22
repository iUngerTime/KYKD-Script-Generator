namespace GeneratorCore.Models;

public class TimeWindow
{
	public TimeOnly Start { get; set; }
	public TimeOnly End { get; set; }

	public TimeWindow(TimeOnly start, TimeOnly end)
	{
		Start = start;
		End = end;
	}
}
