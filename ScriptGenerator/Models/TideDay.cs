using System.Text;
using ScriptGenerator.Models.Enums;

namespace ScriptGenerator.Models;

public class TideDay
{
	public DateOnly Date { get; set; }
	public List<TideInfo> Tides { get; set; } = new List<TideInfo>();
	public Village RelatedVillage { get; set; }

	public TideDay(DateOnly date, Village village)
	{
		Date = date;
		RelatedVillage = village;
	}

	public void AddTide(TideInfo tide)
	{
		Tides.Add(tide);
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"Date: {Date.ToShortDateString()}");
		foreach (var tide in Tides)
		{
			sb.AppendLine($"\t{tide.ToString()}");
		}
		return sb.ToString();
	}
}
